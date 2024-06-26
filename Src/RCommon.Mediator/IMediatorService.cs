﻿using RCommon.EventHandling;

namespace RCommon.Mediator
{
    public interface IMediatorService
    {
        Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default);

        Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default);

        Task<TResponse> Send<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default);
    }
}