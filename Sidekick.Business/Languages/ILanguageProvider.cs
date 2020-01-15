using Sidekick.Business.Languages.Implementations;
using System.Threading.Tasks;

namespace Sidekick.Business.Languages
{
    public interface ILanguageProvider
    {
        LanguageEnum Current { get; }
        ILanguage Language { get; }

        Task<bool> FindAndSetLanguageProvider(string itemDescription);
    }
}
