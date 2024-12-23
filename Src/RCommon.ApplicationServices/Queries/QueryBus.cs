﻿// The MIT License (MIT)
// 
// Copyright (c) 2015-2021 Rasmus Mikkelsen
// Copyright (c) 2015-2021 eBay Software Foundation
// https://github.com/eventflow/EventFlow
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
// the Software, and to permit persons to whom the Software is furnished to do so,
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
// CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RCommon.ApplicationServices.Validation;
using RCommon.Caching;
using RCommon.Models.Queries;
using RCommon.Reflection;

namespace RCommon.ApplicationServices.Queries
{
    public class QueryBus : IQueryBus
    {
        private class HandlerFuncMapping
        {
            public Type QueryHandlerType { get; set; }
            public Func<IQueryHandler, IQuery, CancellationToken, Task> HandlerFunc { get; set; }
        }

        private readonly ILogger<QueryBus> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IValidationService _validationService;
        private readonly IOptions<CqrsValidationOptions> _validationOptions;
        private readonly CachingOptions _cachingOptions;
        private ICacheService _cacheService;

        public QueryBus(ILogger<QueryBus> logger, IServiceProvider serviceProvider, IValidationService validationService,
            IOptions<CqrsValidationOptions> validationOptions, IOptions<CachingOptions> cachingOptions)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _validationService = validationService;
            _validationOptions = validationOptions;
            _cachingOptions = cachingOptions.Value;
        }

        public async Task<TResult> DispatchQueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            if (_validationOptions.Value != null && _validationOptions.Value.ValidateQueries)
            {
                // TODO: Would be nice to be able to take validation outcome and put in IQuery. Need some casting magic
                await _validationService.ValidateAsync(query, true, cancellationToken);
            }

            var queryType = query.GetType();
            var handlerFunc = GetHandlerFuncMapping(queryType);

            var queryHandler = (IQueryHandler)_serviceProvider.GetRequiredService(handlerFunc.QueryHandlerType);
            if (_logger.IsEnabled(LogLevel.Trace))
            {
                _logger.LogTrace(
                    "Executing query {QueryType} ({QueryHandlerType}) by using query handler {QueryHandlerType}",
                    queryType.PrettyPrint(),
                    handlerFunc.QueryHandlerType.PrettyPrint(),
                    queryHandler.GetType().PrettyPrint());
            }

            var task = (Task<TResult>)handlerFunc.HandlerFunc(queryHandler, query, cancellationToken);

            return await task.ConfigureAwait(false);
        }

        private HandlerFuncMapping GetHandlerFuncMapping(Type queryType)
        {
            if (_cachingOptions.CachingEnabled && _cachingOptions.CacheDynamicallyCompiledExpressions)
            {
                var cachingFactory = _serviceProvider.GetService<ICommonFactory<ExpressionCachingStrategy, ICacheService>>();
                Guard.Against<InvalidCacheException>(cachingFactory == null, "We could not properly inject the caching factory: 'ICommonFactory<ExpressionCachingStrategy, ICacheService>>' into the QueryBus");
                _cacheService = cachingFactory.Create(ExpressionCachingStrategy.Default);
                return _cacheService.GetOrCreate(CacheKey.With(GetType(), queryType.GetCacheKey()), 
                    () => this.BuildHandlerFuncMapping(queryType));
            }
            return this.BuildHandlerFuncMapping(queryType);
            
        }

        private HandlerFuncMapping BuildHandlerFuncMapping(Type queryType)
        {
            var queryInterfaceType = queryType
                        .GetTypeInfo()
                        .GetInterfaces()
                        .Single(i => i.GetTypeInfo().IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>));
            var queryHandlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType, queryInterfaceType.GetTypeInfo().GetGenericArguments()[0]);
            var invokeExecuteQueryAsync = ReflectionHelper.CompileMethodInvocation<Func<IQueryHandler, IQuery, CancellationToken, Task>>(
                queryHandlerType,
                "HandleAsync",
                queryType, typeof(CancellationToken));
            return new HandlerFuncMapping
            {
                QueryHandlerType = queryHandlerType,
                HandlerFunc = invokeExecuteQueryAsync
            };
        }
    }
}
