using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Languages;
using Sidekick.Core.Settings;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Leagues;
using Sidekick.Helpers;
using Sidekick.Localization;

namespace Sidekick.Setup
{
    public class SetupViewModel : INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly ILanguageProvider languageProvider;
        private readonly SidekickSettings sidekickSettings;
        private readonly IMediator mediator;

        public SetupViewModel(
            IUILanguageProvider uiLanguageProvider,
            ILanguageProvider languageProvider,
            SidekickSettings sidekickSettings,
            IMediator mediator)
        {
            this.uiLanguageProvider = uiLanguageProvider;
            this.languageProvider = languageProvider;
            this.sidekickSettings = sidekickSettings;
            this.mediator = mediator;
            Settings = new SidekickSettings();
            AssignValues(sidekickSettings, Settings);

            uiLanguageProvider.AvailableLanguages.ForEach(x => UILanguageOptions.Add(x.NativeName.First().ToString().ToUpper() + x.NativeName.Substring(1), x.Name));
            languageProvider.AvailableLanguages.ForEach(x => ParserLanguageOptions.Add(x.Name, x.LanguageCode));
        }

        public async Task Initialize()
        {
            var leagues = await mediator.Send(new GetLeaguesQuery(true));
            leagues.ForEach(x => LeagueOptions.Add(x.Id, x.Text));
        }

        public ObservableDictionary<string, string> LeagueOptions { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> UILanguageOptions { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> ParserLanguageOptions { get; private set; } = new ObservableDictionary<string, string>();

        public SidekickSettings Settings { get; private set; }

        public async Task Save()
        {
            AssignValues(Settings, sidekickSettings);
            sidekickSettings.HasSetupCompleted = true;
            uiLanguageProvider.SetLanguage(Settings.Language_UI);
            languageProvider.SetLanguage(Settings.Language_Parser);
            sidekickSettings.Save();
            await mediator.Send(new InitializeCommand(false));
        }

        private void AssignValues(SidekickSettings src, SidekickSettings dest)
        {
            // iterates through src Settings (properties) and copies them to dest settings (properties)
            // If there ever comes a time, where some properties do not have to be copied, we can add attributes to exclude them
            src.GetType().GetProperties().ToList().ForEach(x => x.SetValue(dest, x.GetValue(src)));
        }

        public async Task Close()
        {
            await mediator.Send(new ShutdownCommand());
        }
    }
}
