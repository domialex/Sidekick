using System.Threading.Tasks;

namespace Sidekick.Business.Languages
{
    public interface ILanguageProvider
    {
        ILanguage EnglishLanguage { get; }

        bool IsEnglish { get; }

        ILanguage Language { get; }

        LanguageAttribute Current { get; }
    }
}
