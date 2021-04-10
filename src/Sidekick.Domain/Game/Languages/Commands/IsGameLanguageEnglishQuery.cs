using MediatR;

namespace Sidekick.Domain.Game.Languages.Commands
{
    /// <summary>
    /// Determines if the current selected language is english
    /// </summary>
    public class IsGameLanguageEnglishQuery : IQuery<bool>
    {
        /// <summary>
        /// Determines if the current selected language is english
        /// </summary>
        public IsGameLanguageEnglishQuery()
        {
        }
    }
}
