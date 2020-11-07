using MediatR;

namespace Sidekick.Domain.Settings.Commands
{
    /// <summary>
    /// Opens the settings view
    /// </summary>
    public class OpenSettingsCommand : ICommand<bool>
    {
    }
}
