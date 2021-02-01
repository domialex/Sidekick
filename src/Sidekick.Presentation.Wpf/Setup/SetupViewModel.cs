using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Localization;
using Sidekick.Application.Settings;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Languages.Commands;
using Sidekick.Domain.Game.Leagues.Queries;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Localization;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Extensions;
using Sidekick.Presentation.Wpf.Helpers;

namespace Sidekick.Presentation.Wpf.Setup
{
    public class SetupViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
#pragma warning restore 67

        private readonly IUILanguageProvider uiLanguageProvider;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly ISidekickSettings sidekickSettings;
        private readonly IMediator mediator;
        private readonly IStringLocalizer localizer;

        public SetupViewModel(
            IUILanguageProvider uiLanguageProvider,
            IGameLanguageProvider gameLanguageProvider,
            ISidekickSettings sidekickSettings,
            IMediator mediator,
            IStringLocalizer<SetupViewModel> localizer)
        {
            this.uiLanguageProvider = uiLanguageProvider;
            this.gameLanguageProvider = gameLanguageProvider;
            this.sidekickSettings = sidekickSettings;
            this.mediator = mediator;
            this.localizer = localizer;
            uiLanguageProvider.AvailableLanguages.ForEach(x => UILanguageOptions.Add(x.NativeName.First().ToString().ToUpper() + x.NativeName[1..], x.Name));
            gameLanguageProvider.AvailableLanguages.ForEach(x => ParserLanguageOptions.Add(x.Name, x.LanguageCode));

            sidekickSettings.CopyValuesTo(this);

            _ = InitializeLeagues();
        }

        // This is called when Language_Parser changes, thanks to Fody
        public void OnLanguage_ParserChanged()
        {
            Task.Run(async () =>
            {
                await mediator.Send(new SetGameLanguageCommand(Language_Parser));
                await InitializeLeagues();
            });
        }

        public async Task InitializeLeagues()
        {
            if (gameLanguageProvider.Language != null)
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
            var newSettings = new SidekickSettings();
            sidekickSettings.CopyValuesTo(newSettings);
            this.CopyValuesTo(newSettings);

            await mediator.Send(new SaveSettingsCommand(newSettings));
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
                    if (string.IsNullOrEmpty(Language_UI)) result.Add(localizer["Validation_Language_UI"]);
                    break;
                case nameof(Language_Parser):
                    if (string.IsNullOrEmpty(Language_Parser)) result.Add(localizer["Validation_Language_Parser"]);
                    break;
                case nameof(LeagueId):
                    if (string.IsNullOrEmpty(LeagueId)) result.Add(localizer["Validation_LeagueId"]);
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
