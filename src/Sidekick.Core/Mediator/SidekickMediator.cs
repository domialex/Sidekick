using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace Sidekick.Core.Mediator
{
    public class SidekickMediator : IMediator
    {
        private readonly ILogger logger;
        private readonly MediatR.Mediator mediator;

        public SidekickMediator(
            ILogger logger,
            ServiceFactory serviceFactory)
        {
            this.logger = logger;
            mediator = new MediatR.Mediator(serviceFactory);
        }

        public async Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
             where TNotification : INotification
        {
            var stopwatch = Stopwatch.StartNew();
            var requestNameWithGuid = $"[{Guid.NewGuid().ToString().Substring(0, 8)}] {notification?.GetType()?.Name}";

            logger.Information($"[MediatR:EVENT] {requestNameWithGuid}");

            try
            {
                logger.Information($"[MediatR:PROPS] {requestNameWithGuid} {JsonSerializer.Serialize(notification)}");
            }
            catch (Exception)
            {
                logger.Information($"[MediatR:ERROR] {requestNameWithGuid} Could not serialize the notification.");
            }

            await mediator.Publish<TNotification>(notification, cancellationToken);

            stopwatch.Stop();
            logger.Information($"[MediatR:END]   {requestNameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
        }

        public async Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestNameWithGuid = $"{ notification?.GetType()?.Name} [{Guid.NewGuid().ToString().Substring(0, 8)}]";

            logger.Information($"[MediatR:EVENT] {requestNameWithGuid}");

            try
            {
                logger.Information($"[MediatR:PROPS] {requestNameWithGuid} {JsonSerializer.Serialize(notification)}");
            }
            catch (Exception)
            {
                logger.Information($"[MediatR:ERROR] {requestNameWithGuid} Could not serialize the notification.");
            }

            await mediator.Publish(notification, cancellationToken);

            stopwatch.Stop();
            logger.Information($"[MediatR:END]   {requestNameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
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
