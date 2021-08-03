using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Common.Settings;
using Sidekick.Domain.Game.Maps.Commands;
using Sidekick.Domain.Game.Maps.Models;
using Sidekick.Domain.Game.Modifiers.Models;

namespace Sidekick.Application.Game.Maps
{
    public class GetMapInfoHandler : IQueryHandler<GetMapInfo, MapInfo>
    {
        private readonly ISettings settings;

        public GetMapInfoHandler(
            ISettings settings)
        {
            this.settings = settings;
        }

        public Task<MapInfo> Handle(GetMapInfo request, CancellationToken cancellationToken)
        {
            var info = new MapInfo()
            {
                Item = request.Item,
                DangerousMods = new List<string>(),
                OkMods = new List<string>(),
            };

            var dangerousModsRegex = new Regex(settings.Map_Dangerous_Regex, RegexOptions.IgnoreCase);
            FilterMods(info, dangerousModsRegex, request.Item.Modifiers.Enchant);
            FilterMods(info, dangerousModsRegex, request.Item.Modifiers.Implicit);
            FilterMods(info, dangerousModsRegex, request.Item.Modifiers.Explicit);
            FilterMods(info, dangerousModsRegex, request.Item.Modifiers.Fractured);
            FilterMods(info, dangerousModsRegex, request.Item.Modifiers.Crafted);

            return Task.FromResult(info);
        }

        private void FilterMods(MapInfo info, Regex modRegex, List<Modifier> modifiers)
        {
            foreach (var mod in modifiers)
            {
                if (modRegex.IsMatch(mod.Text))
                {
                    info.DangerousMods.Add(mod.Text);
                }
                else
                {
                    info.OkMods.Add(mod.Text);
                }
            }
        }
    }
}
