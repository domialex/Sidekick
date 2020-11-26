using System.Collections.Generic;
using System.Linq;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Business.Apis.Poe.Trade.Search.Results
{
    public class TradeItem : Item
    {
        public TradeItem(Result result, IStatDataService statDataService)
        {
            Id = result.Id;
            Listing = result.Listing;

            Corrupted = result.Item.Corrupted;
            Identified = result.Item.Identified;
            Influences = result.Item.Influences;
            ItemLevel = result.Item.ItemLevel;
            Name = result.Item.Name;
            NameLine = result.Item.Name;
            Rarity = result.Item.Rarity;
            Sockets = result.Item.Sockets;
            Type = result.Item.TypeLine;
            TypeLine = result.Item.TypeLine;

            Icon = result.Item.Icon;
            Note = result.Item.Note;
            PropertyTexts = result.Item.Properties;
            Requirements = result.Item.Requirements;

            Properties.Armor = result.Item.Extended.ArmourAtMax;
            Properties.EnergyShield = result.Item.Extended.EnergyShieldAtMax;
            Properties.Evasion = result.Item.Extended.EvasionAtMax;
            Properties.DamagePerSecond = result.Item.Extended.DamagePerSecond;
            Properties.ElementalDps = result.Item.Extended.ElementalDps;
            Properties.PhysicalDps = result.Item.Extended.PhysicalDps;

            ParseMods(statDataService,
                Modifiers.Crafted,
                result.Item.CraftedMods,
                result.Item.Extended.Mods?.Crafted,
                result.Item.Extended.Hashes?.Crafted);

            ParseMods(statDataService,
                Modifiers.Enchant,
                result.Item.EnchantMods,
                result.Item.Extended.Mods?.Enchant,
                result.Item.Extended.Hashes?.Enchant);

            ParseMods(statDataService,
                Modifiers.Explicit,
                result.Item.ExplicitMods,
                result.Item.Extended.Mods?.Explicit,
                result.Item.Extended.Hashes?.Explicit);

            ParseMods(statDataService,
                Modifiers.Implicit,
                result.Item.ImplicitMods,
                result.Item.Extended.Mods?.Implicit,
                result.Item.Extended.Hashes?.Implicit);

            ParseMods(statDataService,
                Modifiers.Pseudo,
                result.Item.PseudoMods,
                result.Item.Extended.Mods?.Pseudo,
                result.Item.Extended.Hashes?.Pseudo);

            ParseMods(statDataService,
                Modifiers.Fractured,
                result.Item.FracturedMods,
                result.Item.Extended.Mods?.Fractured,
                result.Item.Extended.Hashes?.Fractured);
        }

        private void ParseMods(IStatDataService statDataService, List<Modifier> modifiers, List<string> texts, List<Mod> mods, List<LineContentValue> hashes)
        {
            if (modifiers == null || mods == null || hashes == null)
            {
                return;
            }

            for (var index = 0; index < hashes.Count; index++)
            {
                var definition = statDataService.GetById(hashes[index].Value);
                if (definition == null)
                {
                    continue;
                }

                var text = texts.FirstOrDefault(x => (definition.Pattern != null && definition.Pattern.IsMatch(x))
                    || (definition.NegativePattern != null && definition.NegativePattern.IsMatch(x)));
                var mod = mods.FirstOrDefault(x => x.Magnitudes != null && x.Magnitudes.Any(y => y.Hash == definition.Id));

                modifiers.Add(new Modifier()
                {
                    Id = definition.Id,
                    Text = text ?? definition.Text,
                    Tier = mod?.Tier,
                    TierName = mod?.Name,
                });
            }
        }

        public string Id { get; set; }
        public Listing Listing { get; set; }

        public string Icon { get; set; }
        public string Note { get; set; }
        public List<LineContent> PropertyTexts { get; set; }
        public List<LineContent> Requirements { get; set; }

    }
}
