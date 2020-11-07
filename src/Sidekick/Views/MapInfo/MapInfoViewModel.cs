using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Settings;
using Sidekick.Helpers;

namespace Sidekick.Views.MapInfo
{
    public class MapInfoViewModel : INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        private readonly IClipboardProvider clipboardProvider;
        private readonly IParserService parserService;

        public MapInfoViewModel(
            IClipboardProvider clipboardProvider,
            IParserService parserService,
            ISidekickSettings settings)
        {
            this.clipboardProvider = clipboardProvider;
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

        private void Initialize()
        {
            Item = parserService.ParseItem(clipboardProvider.LastCopiedText);

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
