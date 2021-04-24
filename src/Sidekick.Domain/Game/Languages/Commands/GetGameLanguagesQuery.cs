using System.Collections.Generic;
using MediatR;

namespace Sidekick.Domain.Game.Languages.Commands
{
    /// <summary>
    /// Gets the available language for Path of Exile
    /// </summary>
    public class GetGameLanguagesQuery : IQuery<List<GameLanguageAttribute>>
    {
        /// <summary>
        /// Gets the available language for Path of Exile
        /// </summary>
        public GetGameLanguagesQuery()
        {
        }
    }
}
