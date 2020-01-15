using Sidekick.Business.Languages.Implementations;
using System;
using System.Threading.Tasks;

namespace Sidekick.Business.Languages
{
    public interface ILanguageProvider
    {
        LanguageEnum Current { get; }
        ILanguage Language { get; }
        event Func<Task> LanguageChanged;

        Task<bool> FindAndSetLanguageProvider(string itemDescription);
    }
}
