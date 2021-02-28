using Sidekick.Domain.Game.Items;

namespace Sidekick.Application.Game.Items.Parser.Extensions
{
    public static class ParsingItemExtensions
    {
        public static bool TryGetMapTierLine(this ParsingItem item, out string mapTierLine)
        {
            if (item.SplitSections.Length > 1)
            {
                mapTierLine = item.SplitSections[1][0];
                return true;
            }

            mapTierLine = null;
            return false;
        }
    }
}
