using System.Collections.Generic;

namespace Sidekick.Common.Game.Languages
{
    public interface IGameLanguageProvider
    {
        IGameLanguage Language { get; set; }

        void SetLanguage(string languageCode);

        List<GameLanguageAttribute> GetList();

        IGameLanguage Get(string code);

        bool IsEnglish();
    }
}
