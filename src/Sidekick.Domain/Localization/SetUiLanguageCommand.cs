using MediatR;

namespace Sidekick.Domain.Localization
{
    /// <summary>
    /// Sets the Ui language
    /// </summary>
    public class SetUiLanguageCommand : ICommand
    {
        /// <summary>
        /// Sets the Ui language
        /// </summary>
        /// <param name="name">The culture name of the desired language</param>
        public SetUiLanguageCommand(string name)
        {
            Name = name;
        }

        /// <summary>
        /// The culture name of the desired language
        /// </summary>
        public string Name { get; }
    }
}
