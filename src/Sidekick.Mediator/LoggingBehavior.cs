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
            var guid = $"[{Guid.NewGuid().ToString().Substring(0, 8)}]";
            var nameWithGuid = $"{guid} {request.GetType().FullName}";

            TResponse response;

            var stopwatch = Stopwatch.StartNew();
            try
            {
                logger.LogInformation($"[MediatR:START] {nameWithGuid}");

                try
                {
                    logger.LogInformation($"[MediatR:PROPS] {guid} {JsonSerializer.Serialize(request)}");
                }
                catch (Exception)
                {
                    logger.LogInformation($"[MediatR:ERROR] {guid} Could not serialize the request.");
                }

                response = await next();
            }
            finally
            {
                stopwatch.Stop();
                logger.LogInformation($"[MediatR:END]   {nameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
            }

            return response;
        }
    }
}
