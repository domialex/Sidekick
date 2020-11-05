using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Sidekick.Mediator.Internal
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
            var stopwatch = Stopwatch.StartNew();
            TResponse response;

            try
            {
                logger.LogInformation($"[Mediator:START] {nameWithGuid}");

                try
                {
                    logger.LogInformation($"[Mediator:PROPS] {guid} {JsonSerializer.Serialize(request)}");
                }
                catch (Exception)
                {
                    logger.LogInformation($"[Mediator:ERROR] {guid} Could not serialize the request.");
                }

                response = await next();
            }
            catch (Exception e)
            {
                logger.LogInformation($"[Mediator:ERROR] {nameWithGuid} - {e.Message}");
                throw;
            }
            finally
            {
                stopwatch.Stop();
                logger.LogInformation($"[Mediator:END]   {nameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
            }

            return response;
        }
    }
}
