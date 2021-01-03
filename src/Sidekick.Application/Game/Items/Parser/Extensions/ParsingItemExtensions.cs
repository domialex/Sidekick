using Sidekick.Domain.Game.Items;

namespace Sidekick.Application.Game.Items.Parser.Extensions
{
    public static class ParsingItemExtensions
    {
        public static bool TryGetVaalGemName(this ParsingItem item, out string gemName)
        {
            if (item.SplitSections.Length > 7)
            {
                gemName = item.SplitSections[5][0];
                return true;
            }

            gemName = null;
            return false;
        }


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
