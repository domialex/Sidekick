using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Settings;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Languages;
using Sidekick.Domain.Languages.Commands;
using Sidekick.Domain.Leagues;
using Sidekick.Extensions;
using Sidekick.Helpers;
using Sidekick.Localization;

namespace Sidekick.Setup
{
    public class SetupViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
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

            uiLanguageProvider.AvailableLanguages.ForEach(x => UILanguageOptions.Add(x.NativeName.First().ToString().ToUpper() + x.NativeName.Substring(1), x.Name));
            languageProvider.AvailableLanguages.ForEach(x => ParserLanguageOptions.Add(x.Name, x.LanguageCode));

            sidekickSettings.CopyValuesTo(this);

            _ = InitializeLeagues();
        }

        // This is called when Language_Parser changes, thanks to Fody
        public void OnLanguage_ParserChanged()
        {
            Task.Run(async () =>
            {
                await mediator.Send(new SetLanguageCommand(Language_Parser));
                await InitializeLeagues();
            });
        }

        public async Task InitializeLeagues()
        {
            if (languageProvider.Language != null)
            {
                var leagues = await mediator.Send(new GetLeaguesQuery(false));
                LeagueOptions.Clear();
                leagues.ForEach(x => LeagueOptions.Add(x.Id, x.Text));
            }
        }

        public ObservableDictionary<string, string> LeagueOptions { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> UILanguageOptions { get; private set; } = new ObservableDictionary<string, string>();

        public ObservableDictionary<string, string> ParserLanguageOptions { get; private set; } = new ObservableDictionary<string, string>();

        public string Language_UI { get; set; }
        public string Language_Parser { get; set; }
        public string LeagueId { get; set; }
        public string Character_Name { get; set; }

        public async Task Save()
        {
            this.CopyValuesTo(sidekickSettings);
            uiLanguageProvider.SetLanguage(Language_UI);
            await mediator.Send(new SetLanguageCommand(Language_Parser));
            sidekickSettings.Save();
            await mediator.Send(new InitializeCommand(true));
        }

        public async Task Close()
        {
            await mediator.Send(new ShutdownCommand());
        }

        public List<string> GetErrors(string propertyName)
        {
            var result = new List<string>();

            switch (propertyName)
            {
                case nameof(Language_UI):
                    if (string.IsNullOrEmpty(Language_UI)) result.Add("Language is required.");
                    break;
                case nameof(Language_Parser):
                    if (string.IsNullOrEmpty(Language_Parser)) result.Add("Language is required.");
                    break;
                case nameof(LeagueId):
                    if (string.IsNullOrEmpty(LeagueId)) result.Add("A league is required.");
                    break;
                case nameof(Character_Name):
                    break;
            }

            return result;
        }

        public bool HasErrors => GetType().GetProperties().Any(x => GetErrors(x.Name).Count > 0);
        IEnumerable INotifyDataErrorInfo.GetErrors(string propertyName) => GetErrors(propertyName);
    }
}
