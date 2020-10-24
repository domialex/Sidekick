using MediatR;

namespace MediatR
{
    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
