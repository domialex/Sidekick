using System;
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

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
             where TNotification : INotification
        {
            Log(notification);
            return mediator.Publish<TNotification>(notification, cancellationToken);
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            Log(notification);
            return mediator.Publish(notification, cancellationToken);
        }

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return mediator.Send<TResponse>(request, cancellationToken);
        }

        public Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            return mediator.Send(request, cancellationToken);
        }

        private void Log(object notification)
        {
            var requestNameWithGuid = $"{ notification?.GetType()?.Name} [{Guid.NewGuid().ToString().Substring(0, 8)}]";

            logger.Information($"MediatR[PUBLISH] {requestNameWithGuid}");

            try
            {
                logger.Information($"MediatR[PROPS] {requestNameWithGuid} {JsonSerializer.Serialize(notification)}");
            }
            catch (Exception)
            {
                logger.Information($"MediatR[ERROR] {requestNameWithGuid} Could not serialize the notification.");
            }
        }
    }
}
