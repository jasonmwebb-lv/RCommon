﻿using MediatR;
using Microsoft.Extensions.Logging;
using RCommon.DataServices;
using RCommon.DataServices.Transactions;
using RCommon.Extensions;
using Serilog.Context;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RCommon.ApplicationServices.MediatR.Behaviors
{
    public class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger<UnitOfWorkBehavior<TRequest, TResponse>> _logger;
        private readonly IUnitOfWorkScopeFactory _unitOfWorkScopeFactory;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkBehavior(IUnitOfWorkScopeFactory unitOfWorkScopeFactory, IUnitOfWorkManager unitOfWorkManager,
            ILogger<UnitOfWorkBehavior<TRequest, TResponse>> logger)
        {
            _unitOfWorkScopeFactory = unitOfWorkScopeFactory ?? throw new ArgumentException(nameof(IUnitOfWorkScopeFactory));
            _unitOfWorkManager = unitOfWorkManager  ?? throw new ArgumentException(nameof(IUnitOfWorkManager)); 
            _logger = logger ?? throw new ArgumentException(nameof(ILogger));
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var typeName = request.GetGenericTypeName();

            try
            {
                if (this._unitOfWorkManager.CurrentUnitOfWork == null)
                {
                    return await next();
                }

                using (var unitOfWork = this._unitOfWorkScopeFactory.Create(TransactionMode.Default))
                using (LogContext.PushProperty("TransactionContext", this._unitOfWorkManager.CurrentUnitOfWork.TransactionId))
                {
                    _logger.LogInformation("----- Begin transaction {UnitOfWorkTransactionId} for {CommandName} ({@Command})", 
                        this._unitOfWorkManager.CurrentUnitOfWork.TransactionId, typeName, request);

                    response = await next();

                    _logger.LogInformation("----- Commit transaction {UnitOfWorkTransactionId} for {CommandName}", 
                        this._unitOfWorkManager.CurrentUnitOfWork.TransactionId, typeName);

                    unitOfWork.Commit();
                }

                //Perform MassTransit publish events
                


                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", typeName, request);

                throw;
            }
        }
    }
}