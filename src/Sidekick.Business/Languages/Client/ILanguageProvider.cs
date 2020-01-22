using System.Threading.Tasks;

namespace Sidekick.Business.Languages.Client
{
    public interface ILanguageProvider
    {
        string DefaultLanguage { get; }

        bool IsEnglish { get; }

        ILanguage Language { get; }

        LanguageAttribute Current { get; }

        Task FindAndSetLanguage(string itemDescription);
    }
}
