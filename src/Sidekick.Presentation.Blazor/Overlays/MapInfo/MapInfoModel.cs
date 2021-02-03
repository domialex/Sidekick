using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Settings;
using System.Linq;
using Sidekick.Extensions;
using System.Text.Json;

namespace Sidekick.Presentation.Blazor.Overlays.MapInfo
{
    public class MapInfoModel : IDisposable
    {

        public MapInfoModel(ISidekickSettings settings)
        {
            DangerousModsRegex = new Regex(
                settings.Map_Dangerous_Regex,
                RegexOptions.Compiled | RegexOptions.IgnoreCase);

            DangerousMapMods = new List<String>();
            NewLinePattern = new Regex("(?:\\\\)*[\\r\\n]+");
        }

        public Item Item { get; private set; }
        public bool IsSafe { get; private set; }
        public List<String> DangerousMapMods { get; private set; }

        private Regex DangerousModsRegex { get; set; }
        private Regex NewLinePattern { get; set; }

        public void Initialize(string encodedItem)
        {
            this.Initialize(JsonSerializer.Deserialize<Item>(encodedItem.DecodeUrl().DecodeBase64()));
        }

        public void Initialize(Item item)
        {
            Item = item;

            foreach (var matchingLine in NewLinePattern.Split(Item.Text)
                .Where(line => DangerousModsRegex.IsMatch(line)))
            {
                DangerousMapMods.Add(matchingLine);
            }
            IsSafe = DangerousMapMods.Count == 0;
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
