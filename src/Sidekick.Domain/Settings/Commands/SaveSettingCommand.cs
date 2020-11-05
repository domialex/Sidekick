using MediatR;

namespace Sidekick.Domain.Settings.Commands
{
    /// <summary>
    /// Command to save a single setting. To save all settings at once, use SaveSettingsCommand.
    /// </summary>
    public class SaveSettingCommand : ICommand
    {
        /// <summary>
        /// Command to save a single setting. To save all settings at once, use SaveSettingsCommand.
        /// </summary>
        /// <param name="property">The property to update in the settings.</param>
        /// <param name="value">The value of the setting.</param>
        public SaveSettingCommand(string property, object value)
        {
            Property = property;
            Value = value;
        }

        /// <summary>
        /// The property to update in the settings.
        /// </summary>
        public string Property { get; }

        /// <summary>
        /// The value of the setting.
        /// </summary>
        public object Value { get; }
    }
}
