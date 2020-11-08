using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Extensions
{
    public static class ItemExtensions
    {
        public static string GetColor(this Item item)
        {
            return item?.Rarity switch
            {
                Rarity.Normal => "#c8c8c8",
                Rarity.Magic => "#8888ff",
                Rarity.Rare => "#ffff77",
                Rarity.Unique => "#af6025",
                _ => "#aa9e82",
            };
        }

        public static string GetColor(this Rarity item)
        {
            return item switch
            {
                Rarity.Normal => "#c8c8c8",
                Rarity.Magic => "#8888ff",
                Rarity.Rare => "#ffff77",
                Rarity.Unique => "#af6025",
                _ => "#aa9e82",
            };
        }
    }
}
