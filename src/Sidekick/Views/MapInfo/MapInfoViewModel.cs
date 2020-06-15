using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using System.Text.RegularExpressions;
using Sidekick.Helpers;
using System.ComponentModel;

namespace Sidekick.Views.MapInfo
{
    public class MapInfoViewModel : INotifyPropertyChanged
    {
        private readonly INativeClipboard nativeClipboard;
        private readonly IParserService parserService;

        public event PropertyChangedEventHandler PropertyChanged;

        public MapInfoViewModel(
            INativeClipboard nativeClipboard,
            IParserService parserService,
            SidekickSettings settings)
        {
            this.nativeClipboard = nativeClipboard;
            this.parserService = parserService;
            DangerousModsRegex = new Regex(
                settings.DangerousModsRegex,
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            DangerousMapMods = new ObservableList<DangerousMapModModel>();
            NewLinePattern = new Regex("(?:\\\\)*[\\r\\n]+");
            IsParsing = true;
            Task.Run(Initialize);
        }

        public Item Item { get; private set; }
        public bool IsError { get; private set; }
        public bool IsParsing { get; private set; }
        public bool IsSafe { get; private set; }
        public ObservableList<DangerousMapModModel> DangerousMapMods { get; private set; }

        private Regex DangerousModsRegex { get; set; }
        private Regex NewLinePattern { get; set; }

        private async Task Initialize()
        {
            Item = await parserService.ParseItem(nativeClipboard.LastCopiedText);

            if (Item == null || Item.Properties.MapTier == 0)
            {
                IsError = true;
                return;
            }

            foreach (var matchingLine in NewLinePattern.Split(Item.Text)
                .Where(line => DangerousModsRegex.IsMatch(line)))
            {
                DangerousMapMods.Add(new DangerousMapModModel(matchingLine, "#ff2222"));
            }
            IsSafe = DangerousMapMods.Count == 0;
            IsParsing = false;
        }
    }
}
