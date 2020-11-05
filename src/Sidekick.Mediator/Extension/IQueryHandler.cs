namespace MediatR
{
    public interface IQueryHandler<in TQuery, TResponse> : MediatR.IRequestHandler<TQuery, TResponse>
        where TQuery : IQuery<TResponse>
    {
    }
}
