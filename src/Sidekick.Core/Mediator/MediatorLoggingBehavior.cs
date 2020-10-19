using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Serilog;

namespace Sidekick.Core.Mediator
{
    public class MediatorLoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger logger;

        public MediatorLoggingBehavior(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var requestNameWithGuid = $"{ request.GetType().Name} [{Guid.NewGuid().ToString().Substring(0, 8)}]";

            logger.Information($"MediatR[START] {requestNameWithGuid}");
            TResponse response;

            var stopwatch = Stopwatch.StartNew();
            try
            {
                try
                {
                    logger.Information($"MediatR[PROPS] {requestNameWithGuid} {JsonSerializer.Serialize(request)}");
                }
                catch (Exception)
                {
                    logger.Information($"MediatR[ERROR] {requestNameWithGuid} Could not serialize the request.");
                }

                response = await next();
            }
            finally
            {
                stopwatch.Stop();
                logger.Information($"MediatR[END] {requestNameWithGuid}; Execution time={stopwatch.ElapsedMilliseconds}ms");
            }

            return response;
        }
    }
}
