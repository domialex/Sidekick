using MediatR;

namespace Sidekick.Domain.Views.Commands
{
    /// <summary>
    /// Close all opened views
    /// </summary>
    public class CloseViewCommand : ICommand<bool>
    {
    }
}
