using System.Threading.Tasks;

namespace Sidekick.Business.Languages
{
    public interface ILanguageProvider
    {
        ILanguage DefaultLanguage { get; }

        bool IsEnglish { get; }

        ILanguage Language { get; }

        LanguageAttribute Current { get; }

        Task FindAndSetLanguage(string itemDescription);
    }
}
