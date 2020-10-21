using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Mediator.Internal;

namespace Sidekick.Mediator
{
    public class SidekickMediator : IMediator
    {
        private readonly ILogger logger;
        private readonly MediatorOverride mediator;

        public SidekickMediator(
            ILogger<SidekickMediator> logger,
            ServiceFactory serviceFactory)
        {
            this.logger = logger;
            mediator = new MediatorOverride(serviceFactory, logger);
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
             where TNotification : INotification
        {
            var guid = $"[{Guid.NewGuid().ToString().Substring(0, 8)}]";
            var nameWithGuid = $"{guid} { notification?.GetType()?.FullName}";

            var stopwatch = Stopwatch.StartNew();
            logger.LogInformation($"[MediatR:NOTIF] {nameWithGuid}");

            try
            {
                logger.LogInformation($"[MediatR:PROPS] {guid} {JsonSerializer.Serialize(notification)}");
            }
            catch (Exception)
            {
                logger.LogInformation($"[MediatR:ERROR] {guid} Could not serialize the notification.");
            }

            await mediator.Publish<TNotification>(notification, cancellationToken);

            stopwatch.Stop();
            logger.LogInformation($"[MediatR:END]   {nameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
        }

        public async Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            var guid = $"[{Guid.NewGuid().ToString().Substring(0, 8)}]";
            var nameWithGuid = $"{guid} { notification?.GetType()?.FullName}";

            var stopwatch = Stopwatch.StartNew();
            logger.LogInformation($"[MediatR:NOTIF] {nameWithGuid}");

            try
            {
                logger.LogInformation($"[MediatR:PROPS] {guid} {JsonSerializer.Serialize(notification)}");
            }
            catch (Exception)
            {
                logger.LogInformation($"[MediatR:ERROR] {guid} Could not serialize the notification.");
            }

            await mediator.Publish(notification, cancellationToken);

            stopwatch.Stop();
            logger.LogInformation($"[MediatR:END]   {nameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return mediator.Send<TResponse>(request, cancellationToken);
        }

        public Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            return mediator.Send(request, cancellationToken);
        }

    }
}
