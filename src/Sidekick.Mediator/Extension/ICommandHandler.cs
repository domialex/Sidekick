namespace MediatR
{
    public interface ICommandHandler<in TCommand> : MediatR.IRequestHandler<TCommand>
        where TCommand : ICommand
    {
    }
}
