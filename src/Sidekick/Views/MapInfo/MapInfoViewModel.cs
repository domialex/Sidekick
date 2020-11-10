using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Settings;
using Sidekick.Helpers;

namespace Sidekick.Views.MapInfo
{
    public class MapInfoViewModel : INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        public MapInfoViewModel(
            ISidekickSettings settings)
        {
            DangerousModsRegex = new Regex(
                settings.DangerousModsRegex,
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            DangerousMapMods = new ObservableList<DangerousMapModModel>();
            NewLinePattern = new Regex("(?:\\\\)*[\\r\\n]+");
        }

        public Item Item { get; private set; }
        public bool IsSafe { get; private set; }
        public ObservableList<DangerousMapModModel> DangerousMapMods { get; private set; }

        private Regex DangerousModsRegex { get; set; }
        private Regex NewLinePattern { get; set; }

        public void Initialize(Item item)
        {
            Item = item;

            foreach (var matchingLine in NewLinePattern.Split(Item.Text)
                .Where(line => DangerousModsRegex.IsMatch(line)))
            {
                DangerousMapMods.Add(new DangerousMapModModel(matchingLine, "#ff2222"));
            }
            IsSafe = DangerousMapMods.Count == 0;
        }
    }
}
