﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace RCommon.EventHandling.Producers
{
    /// <summary>
    /// Responsible for storing transactional events and wiring up all <see cref="ISerializableEvent">local events</see> to 
    /// their appropriate <see cref="IEventProducer"/>
    /// </summary>
    /// <remarks>This should be a scoped dependency.</remarks>
    public class InMemoryTransactionalEventRouter : IEventRouter
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<InMemoryTransactionalEventRouter> _logger;
        private List<ISerializableEvent> _storedTransactionalEvents;

        public InMemoryTransactionalEventRouter(IServiceProvider serviceProvider, ILogger<InMemoryTransactionalEventRouter> logger)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _storedTransactionalEvents = new List<ISerializableEvent>();
        }

        public async Task RouteEventsAsync(IEnumerable<ISerializableEvent> transactionalEvents)
        {
            try
            {
                Guard.IsNotNull(transactionalEvents, nameof(transactionalEvents));

                if (transactionalEvents.Any())
                {
                    _logger.LogInformation($"{this.GetGenericTypeName()} is routing {transactionalEvents.Count().ToString()} transactional events to event producers.");

                    // Seperate Async events from Sync Events
                    var syncEvents = transactionalEvents.Where(x => x is ISyncEvent);
                    var asyncEvents = transactionalEvents.Where(x => x is IAsyncEvent);
                    var eventProducers = _serviceProvider.GetServices<IEventProducer>();

                    if (syncEvents.Any() && asyncEvents.Any())
                    {
                        // Produce the Synchronized Events first
                        _logger.LogInformation($"{this.GetGenericTypeName()} is routing {syncEvents.Count().ToString()} synchronized transactional events.");
                        await this.ProduceSyncEvents(syncEvents, eventProducers).ConfigureAwait(false);

                        // Produce the Async Events
                        _logger.LogInformation($"{this.GetGenericTypeName()} is routing {asyncEvents.Count().ToString()} asynchronous transactional events.");
                        await this.ProduceAsyncEvents(asyncEvents, eventProducers).ConfigureAwait(false);
                    }
                    else
                    {
                        // Send as synchronized by default
                        _logger.LogInformation($"No sync/async events found. {this.GetGenericTypeName()} is routing {syncEvents.Count().ToString()} as synchronized transactional events by default.");
                        await this.ProduceSyncEvents(transactionalEvents, eventProducers).ConfigureAwait(false);
                    }

                }
            }
            catch(EventProductionException ex)
            {
                _logger.LogError(ex, "An error occured while producing events through the EventRouter: {0}.", this.GetGenericTypeName());
                throw;
            }
            catch (Exception ex)
            {
                var message = "An error occured while producing events through the EventRouter: {0}";
                _logger.LogError(ex, message, this.GetGenericTypeName());
                throw new EventProductionException(message,
                    ex,
                    new object[] { this.GetGenericTypeName() });
            }
        }

        private async Task ProduceAsyncEvents(IEnumerable<ISerializableEvent> asyncEvents, IEnumerable<IEventProducer> eventProducers)
        {
            var eventTaskList = new List<Task>();
            foreach (var @event in asyncEvents)
            {
                foreach (var producer in eventProducers)
                {
                    eventTaskList.Add(producer.ProduceEventAsync(@event));
                }
            }
            await Task.WhenAll(eventTaskList);
        }

        private async Task ProduceSyncEvents(IEnumerable<ISerializableEvent> syncEvents, IEnumerable<IEventProducer> eventProducers)
        {
            foreach (var @event in syncEvents)
            {
                _logger.LogDebug($"{this.GetGenericTypeName()} is routing event: {@event}");
                foreach (var producer in eventProducers)
                {
                    await producer.ProduceEventAsync(@event).ConfigureAwait(false);
                }
            }
        }

        public async Task RouteEventsAsync()
        {
            await this.RouteEventsAsync(this._storedTransactionalEvents).ConfigureAwait(false);
            this._storedTransactionalEvents.Clear();
        }

        public void AddTransactionalEvent(ISerializableEvent serializableEvent)
        {
            Guard.IsNotNull(serializableEvent, nameof(serializableEvent));
            _storedTransactionalEvents.Add(serializableEvent);
        }

        public void AddTransactionalEvents(IEnumerable<ISerializableEvent> serializableEvents)
        {
            Guard.IsNotNull(serializableEvents, nameof(serializableEvents));

            foreach (var serializableEvent in serializableEvents)
            {
                this.AddTransactionalEvent(serializableEvent);
            }
        }
    }
}
