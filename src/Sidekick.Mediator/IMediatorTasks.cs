using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Sidekick.Mediator
{
    public interface IMediatorTasks
    {
        Task Notify<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
             where TNotification : INotification;

        Task<TResponse> Query<TResponse>(IQuery<TResponse> query, CancellationToken cancellationToken = default);

        Task<Unit> Command(ICommand command, CancellationToken cancellationToken = default);

        Task WhenAll { get; }
    }
}
