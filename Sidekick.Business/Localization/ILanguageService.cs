using System.Threading.Tasks;
using Sidekick.Business.Localization.Locales;

namespace Sidekick.Business.Localization
{
    public interface ILanguageService
    {
        LanguageEnum Current { get; }
        ILanguage Language { get; }

        Task<bool> FindAndSetLanguageProvider(string itemDescription);
    }
}
