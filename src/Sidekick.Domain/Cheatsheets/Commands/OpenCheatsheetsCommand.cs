using MediatR;

namespace Sidekick.Domain.Cheatsheets.Commands
{
    /// <summary>
    /// Opens the cheatsheet view.
    /// </summary>
    public class OpenCheatsheetsCommand : ICommand<bool>
    {
    }
}
