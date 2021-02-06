using MediatR;

namespace Sidekick.Domain.Game.Languages.Commands
{
    /// <summary>
    /// Get the specified language for Path of Exile
    /// </summary>
    public class GetGameLanguageQuery : IQuery<IGameLanguage>
    {
        /// <summary>
        /// Get the specified language for Path of Exile
        /// </summary>
        /// <param name="code">The language code of the game language</param>
        public GetGameLanguageQuery(string code)
        {
            Code = code;
        }

        /// <summary>
        /// The language code of the game language
        /// </summary>
        public string Code { get; }
    }
}
