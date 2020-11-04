using MediatR;

namespace Sidekick.Domain.Settings.Commands
{
    /// <summary>
    /// Command to save the settings
    /// </summary>
    public class SaveSettingsCommand : ICommand
    {
        /// <summary>
        /// Command to save the settings
        /// </summary>
        /// <param name="settings">The current settings to save</param>
        public SaveSettingsCommand(ISidekickSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// The current settings to save
        /// </summary>
        public ISidekickSettings Settings { get; }
    }
}
