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

namespace Sidekick.Views.MapInfo
{
    public class MapInfoViewModel
    {
        private readonly INativeClipboard nativeClipboard;
        private readonly IParserService parserService;
        private readonly SidekickSettings settings;

        public MapInfoViewModel(
            INativeClipboard nativeClipboard,
            IParserService parserService,
            SidekickSettings settings)
        {
            this.nativeClipboard = nativeClipboard;
            this.parserService = parserService;
            this.settings = settings;
            DangerousModsRegex = new Regex(
                settings.DangerousModsRegex,
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            DangerousMapMods = new List<DangerousMapModModel>();
            NewLinePattern = new Regex("(?:\\\\)*[\\r\\n]+");
            Task.Run(Initialize);
        }

        public Item Item { get; private set; }
        public bool IsError { get; private set; }
        public bool IsNotError => !IsError;
        public bool IsDangerous => DangerousMapMods.Count != 0;
        public bool IsSafe => !IsDangerous;
        public List<DangerousMapModModel> DangerousMapMods { get; private set; }

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
        }
    }
}
