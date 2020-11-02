using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Mediator.Internal;

namespace Sidekick.Mediator
{
    public class Mediator : IMediatorTasks, MediatR.IMediator
    {
        private readonly ILogger logger;
        private readonly LoggingMediator mediator;

        public Mediator(
            ILogger<Mediator> logger,
            ServiceFactory serviceFactory)
        {
            this.logger = logger;
            mediator = new LoggingMediator(serviceFactory, logger);
        }

        private IList<Task> RunningTasks { get; set; } = new List<Task>();

        private Task AddTask(Task task)
        {
            RunAfterHandler(task);
            return task;
        }

        private Task<T> AddTask<T>(Task<T> task)
        {
            RunAfterHandler(task);
            return task;
        }

        private void RunAfterHandler(Task task)
        {
            lock (RunningTasks)
            {
                RunningTasks = RunningTasks.Where(x => !x.IsCanceled && !x.IsFaulted && !x.IsCompleted).ToList();
                RunningTasks.Add(task);
            }
        }

        public Task WhenAll => Task.Run(async () =>
        {
            await Task.WhenAll(RunningTasks.Select(x => x).ToList());
            await Task.Delay(100);
        });

        public Task Notify<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
             where TNotification : INotification
        {
            return Publish(notification, cancellationToken);
        }

        public Task<TResponse> Query<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default)
        {
            return AddTask(Send(query, cancellationToken));
        }

        public Task<Unit> Command(ICommand command, CancellationToken cancellationToken = default)
        {
            return AddTask(mediator.Send(command, cancellationToken));
        }

        #region MediatR interface
        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return AddTask(mediator.Send(request, cancellationToken));
        }

        public Task<object> Send(object request, CancellationToken cancellationToken = default)
        {
            return AddTask(mediator.Send(request, cancellationToken));
        }

        private async Task PublishCore(object notification, CancellationToken cancellationToken = default)
        {
            var guid = $"[{Guid.NewGuid().ToString().Substring(0, 8)}]";
            var nameWithGuid = $"{guid} { notification?.GetType()?.FullName}";
            var stopwatch = Stopwatch.StartNew();

            try
            {
                logger.LogInformation($"[Mediator:NOTIF] {nameWithGuid}");

                try
                {
                    logger.LogInformation($"[Mediator:PROPS] {guid} {JsonSerializer.Serialize(notification)}");
                }
                catch (Exception)
                {
                    logger.LogInformation($"[Mediator:ERROR] {guid} Could not serialize the notification.");
                }

                await AddTask(mediator.Publish(notification, cancellationToken));
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
        }

        public Task Publish(object notification, CancellationToken cancellationToken = default)
        {
            return PublishCore(notification, cancellationToken);
        }

        public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
        {
            return PublishCore(notification, cancellationToken);
        }
        #endregion
    }
}
