using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Sidekick.Mediator.Internal
{
    internal class LoggingMediator : MediatR.Mediator
    {
        private readonly ILogger logger;

        public LoggingMediator(
            ServiceFactory serviceFactory,
            ILogger logger)
            : base(serviceFactory)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Overriding the default PublishCore so that we can log notifications
        /// </summary>
        protected override async Task PublishCore(IEnumerable<Func<INotification, CancellationToken, Task>> allHandlers, INotification notification, CancellationToken cancellationToken)
        {
            foreach (var handler in allHandlers)
            {
                var handlerType = handler?.Target?.GetType().GetFields()?.FirstOrDefault()?.GetValue(handler.Target)?.GetType();
                var handlerNameWithGuid = $"[{Guid.NewGuid().ToString().Substring(0, 8)}] { handlerType?.FullName ?? notification?.GetType()?.FullName}";
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"[Mediator:START] {handlerNameWithGuid}");

                await handler(notification, cancellationToken).ConfigureAwait(false);

                stopwatch.Stop();
                logger.LogInformation($"[Mediator:END]   {handlerNameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
            }
        }
    }
}
