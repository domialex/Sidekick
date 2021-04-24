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
        /// <param name="skipInitialize">Skips initialization process</param>
        public SaveSettingsCommand(ISidekickSettings settings, bool skipInitialize = false)
        {
            Settings = settings;
            SkipInitialize = skipInitialize;
        }

        /// <summary>
        /// The current settings to save
        /// </summary>
        public ISidekickSettings Settings { get; }

        /// <summary>
        /// Skips initialization process
        /// </summary>
        public bool SkipInitialize { get; }
    }
}
