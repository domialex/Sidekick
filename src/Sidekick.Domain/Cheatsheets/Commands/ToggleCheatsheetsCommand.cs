using MediatR;

namespace Sidekick.Domain.Cheatsheets.Commands
{
    /// <summary>
    /// Toggles the cheatsheet view. If the view is opened, we close it. If it is closed, we open it.
    /// </summary>
    public class ToggleCheatsheetsCommand : ICommand<bool>
    {
    }
}
