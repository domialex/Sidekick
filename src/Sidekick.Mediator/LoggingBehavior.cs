using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Sidekick.Mediator
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger logger;

        public LoggingBehavior(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestNameWithGuid = $"[{Guid.NewGuid().ToString().Substring(0, 8)}] {request.GetType().Name}";

            logger.LogInformation($"[MediatR:START] {requestNameWithGuid}");
            TResponse response;

            var stopwatch = Stopwatch.StartNew();
            try
            {
                try
                {
                    logger.LogInformation($"[MediatR:PROPS] {requestNameWithGuid} {JsonSerializer.Serialize(request)}");
                }
                catch (Exception)
                {
                    logger.LogInformation($"[MediatR:ERROR] {requestNameWithGuid} Could not serialize the request.");
                }

                response = await next();
            }
            finally
            {
                stopwatch.Stop();
                logger.LogInformation($"[MediatR:END]   {requestNameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
            }

            return response;
        }
    }
}
