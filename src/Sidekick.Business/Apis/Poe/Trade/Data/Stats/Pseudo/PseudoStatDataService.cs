using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo
{
    public class PseudoStatDataService : IOnInit
    {
        private readonly ILogger logger;
        private readonly IPoeTradeClient poeApiClient;
        private readonly HttpClient client;

        public PseudoStatDataService(ILogger logger,
            IPoeTradeClient poeApiClient,
            IHttpClientFactory httpClientFactory)
        {
            this.logger = logger;
            this.poeApiClient = poeApiClient;
            client = httpClientFactory.CreateClient();
        }

        public async Task OnInit()
        {
            try
            {
                logger.Information($"Pseudo stat service initialization started.");

                var response = await client.GetAsync("https://www.pathofexile.com/api/trade/data/stats/");
                var content = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<FetchResult<StatDataCategory>>(content, poeApiClient.Options);

                logger.Information($"{result.Result.Count} attributes fetched.");

                InitDefinitions();

                foreach (var category in result.Result)
                {
                    var first = category.Entries.FirstOrDefault();
                    if (first == null || first.Id.Split('.').First() == "pseudo")
                    {
                        continue;
                    }

                    foreach (var entry in category.Entries)
                    {
                        foreach (var definition in Definitions.Where(x => x.Value != null))
                        {
                            foreach (var pattern in definition.Value)
                            {
                                if (pattern.Pattern.IsMatch(entry.Text))
                                {
                                    pattern.Matches.Add(entry.Id, entry.Text);
                                }
                            }
                        }
                    }
                }

                // Clean duplicates (Local)
            }
            catch (Exception)
            {
                logger.Information($"Could not initialize pseudo service.");
                throw;
            }
        }

        private Dictionary<string, PseudoPattern[]> Definitions;

        private void InitDefinitions()
        {
            Definitions = new Dictionary<string, PseudoPattern[]>() {
                // +#% total to Cold Resistance
                { "pseudo.pseudo_total_cold_resistance", new [] {
                    new PseudoPattern("(?<!(Minions|Enemies).*)to\\ Cold\\ Resistance"),
                    new PseudoPattern("to\\ .*Cold.*Resistances"),
                }},
                // +#% total to Fire Resistance
                { "pseudo.pseudo_total_fire_resistance", new [] {
                    new PseudoPattern("(?<!(Minions|Enemies).*)to\\ Fire\\ Resistance"),
                    new PseudoPattern("to\\ .*Fire.*Resistances"),
                }},
                // +#% total to Lightning Resistance
                { "pseudo.pseudo_total_lightning_resistance", new [] {
                    new PseudoPattern("(?<!(Minions|Enemies).*)to\\ Lightning\\ Resistance"),
                    new PseudoPattern("to\\ .*Lightning.*Resistances"),
                }},
                // +#% total Elemental Resistance
                { "pseudo.pseudo_total_elemental_resistance", null },
                // +#% total to Chaos Resistance
                { "pseudo.pseudo_total_chaos_resistance", null },
                // +#% total Resistance
                { "pseudo.pseudo_total_resistance", null },
                // # total Resistances
                { "pseudo.pseudo_count_resistances", null },
                // # total Elemental Resistances
                { "pseudo.pseudo_count_elemental_resistances", null },
                // +#% total to all Elemental Resistances
                { "pseudo.pseudo_total_all_elemental_resistances", null },
                // +# total to Strength
                { "pseudo.pseudo_total_strength", null },
                // +# total to Dexterity
                { "pseudo.pseudo_total_dexterity", null },
                // +# total to Intelligence
                { "pseudo.pseudo_total_intelligence", null },
                // +# total to all Attributes
                { "pseudo.pseudo_total_all_attributes", null },
                // +# total maximum Life
                { "pseudo.pseudo_total_life", null },
                // +# total maximum Mana
                { "pseudo.pseudo_total_mana", null },
                // +# total maximum Energy Shield
                { "pseudo.pseudo_total_energy_shield", null },
                // #% total increased maximum Energy Shield
                { "pseudo.pseudo_increased_energy_shield", null },
                // +#% total Attack Speed
                { "pseudo.pseudo_total_attack_speed", null },
                // +#% total Cast Speed
                { "pseudo.pseudo_total_cast_speed", null },
                // #% increased Movement Speed
                { "pseudo.pseudo_increased_movement_speed", null },
                // #% total increased Physical Damage
                { "pseudo.pseudo_increased_physical_damage", null },
                // +#% Global Critical Strike Chance
                { "pseudo.pseudo_global_critical_strike_chance", null },
                // +#% total Critical Strike Chance for Spells
                { "pseudo.pseudo_critical_strike_chance_for_spells", null },
                // +#% Global Critical Strike Multiplier
                { "pseudo.pseudo_global_critical_strike_multiplier", null },
                // Adds # to # Physical Damage
                { "pseudo.pseudo_adds_physical_damage", null },
                // Adds # to # Lightning Damage
                { "pseudo.pseudo_adds_lightning_damage", null },
                // Adds # to # Cold Damage
                { "pseudo.pseudo_adds_cold_damage", null },
                // Adds # to # Fire Damage
                { "pseudo.pseudo_adds_fire_damage", null },
                // Adds # to # Elemental Damage
                { "pseudo.pseudo_adds_elemental_damage", null },
                // Adds # to # Chaos Damage
                { "pseudo.pseudo_adds_chaos_damage", null },
                // Adds # to # Damage
                { "pseudo.pseudo_adds_damage", null },
                // Adds # to # Physical Damage to Attacks
                { "pseudo.pseudo_adds_physical_damage_to_attacks", null },
                // Adds # to # Lightning Damage to Attacks
                { "pseudo.pseudo_adds_lightning_damage_to_attacks", null },
                // Adds # to # Cold Damage to Attacks
                { "pseudo.pseudo_adds_cold_damage_to_attacks", null },
                // Adds # to # Fire Damage to Attacks
                { "pseudo.pseudo_adds_fire_damage_to_attacks", null },
                // Adds # to # Elemental Damage to Attacks
                { "pseudo.pseudo_adds_elemental_damage_to_attacks", null },
                // Adds # to # Chaos Damage to Attacks
                { "pseudo.pseudo_adds_chaos_damage_to_attacks", null },
                // Adds # to # Damage to Attacks
                { "pseudo.pseudo_adds_damage_to_attacks", null },
                // Adds # to # Physical Damage to Spells
                { "pseudo.pseudo_adds_physical_damage_to_spells", null },
                // Adds # to # Lightning Damage to Spells
                { "pseudo.pseudo_adds_lightning_damage_to_spells", null },
                // Adds # to # Cold Damage to Spells
                { "pseudo.pseudo_adds_cold_damage_to_spells", null },
                // Adds # to # Fire Damage to Spells
                { "pseudo.pseudo_adds_fire_damage_to_spells", null },
                // Adds # to # Elemental Damage to Spells
                { "pseudo.pseudo_adds_elemental_damage_to_spells", null },
                // Adds # to # Chaos Damage to Spells
                { "pseudo.pseudo_adds_chaos_damage_to_spells", null },
                // Adds # to # Damage to Spells
                { "pseudo.pseudo_adds_damage_to_spells", null },
                // #% increased Elemental Damage
                { "pseudo.pseudo_increased_elemental_damage", null },
                // #% increased Lightning Damage
                { "pseudo.pseudo_increased_lightning_damage", null },
                // #% increased Cold Damage
                { "pseudo.pseudo_increased_cold_damage", null },
                // #% increased Fire Damage
                { "pseudo.pseudo_increased_fire_damage", null },
                // #% increased Spell Damage
                { "pseudo.pseudo_increased_spell_damage", null },
                // #% increased Lightning Spell Damage
                { "pseudo.pseudo_increased_lightning_spell_damage", null },
                // #% increased Cold Spell Damage
                { "pseudo.pseudo_increased_cold_spell_damage", null },
                // #% increased Fire Spell Damage
                { "pseudo.pseudo_increased_fire_spell_damage", null },
                // #% increased Lightning Damage with Attack Skills
                { "pseudo.pseudo_increased_lightning_damage_with_attack_skills", null },
                // #% increased Cold Damage with Attack Skills
                { "pseudo.pseudo_increased_cold_damage_with_attack_skills", null },
                // #% increased Fire Damage with Attack Skills
                { "pseudo.pseudo_increased_fire_damage_with_attack_skills", null },
                // #% increased Elemental Damage with Attack Skills
                { "pseudo.pseudo_increased_elemental_damage_with_attack_skills", null },
                // #% increased Rarity of Items found
                { "pseudo.pseudo_increased_rarity", null },
                // #% increased Burning Damage
                { "pseudo.pseudo_increased_burning_damage", null },
                // # Life Regenerated per Second
                { "pseudo.pseudo_total_life_regen", null },
                // #% of Life Regenerated per Second
                { "pseudo.pseudo_percent_life_regen", null },
                // #% of Physical Attack Damage Leeched as Life
                { "pseudo.pseudo_physical_attack_damage_leeched_as_life", null },
                // #% of Physical Attack Damage Leeched as Mana
                { "pseudo.pseudo_physical_attack_damage_leeched_as_mana", null },
                // #% increased Mana Regeneration Rate
                { "pseudo.pseudo_increased_mana_regen", null },
                // +# total to Level of Socketed Gems
                { "pseudo.pseudo_total_additional_gem_levels", null },
                // +# total to Level of Socketed Elemental Gems
                { "pseudo.pseudo_total_additional_elemental_gem_levels", null },
                // +# total to Level of Socketed Fire Gems
                { "pseudo.pseudo_total_additional_fire_gem_levels", null },
                // +# total to Level of Socketed Cold Gems
                { "pseudo.pseudo_total_additional_cold_gem_levels", null },
                // +# total to Level of Socketed Lightning Gems
                { "pseudo.pseudo_total_additional_lightning_gem_levels", null },
                // +# total to Level of Socketed Chaos Gems
                { "pseudo.pseudo_total_additional_chaos_gem_levels", null },
                // +# total to Level of Socketed Spell Gems
                { "pseudo.pseudo_total_additional_spell_gem_levels", null },
                // +# total to Level of Socketed Projectile Gems
                { "pseudo.pseudo_total_additional_projectile_gem_levels", null },
                // +# total to Level of Socketed Bow Gems
                { "pseudo.pseudo_total_additional_bow_gem_levels", null },
                // +# total to Level of Socketed Melee Gems
                { "pseudo.pseudo_total_additional_melee_gem_levels", null },
                // +# total to Level of Socketed Minion Gems
                { "pseudo.pseudo_total_additional_minion_gem_levels", null },
                // +# total to Level of Socketed Strength Gems
                { "pseudo.pseudo_total_additional_strength_gem_levels", null },
                // +# total to Level of Socketed Dexterity Gems
                { "pseudo.pseudo_total_additional_dexterity_gem_levels", null },
                // +# total to Level of Socketed Intelligence Gems
                { "pseudo.pseudo_total_additional_intelligence_gem_levels", null },
                // +# total to Level of Socketed Aura Gems
                { "pseudo.pseudo_total_additional_aura_gem_levels", null },
                // +# total to Level of Socketed Movement Gems
                { "pseudo.pseudo_total_additional_movement_gem_levels", null },
                // +# total to Level of Socketed Curse Gems
                { "pseudo.pseudo_total_additional_curse_gem_levels", null },
                // +# total to Level of Socketed Vaal Gems
                { "pseudo.pseudo_total_additional_vaal_gem_levels", null },
                // +# total to Level of Socketed Support Gems
                { "pseudo.pseudo_total_additional_support_gem_levels", null },
                // +# total to Level of Socketed Skill Gems
                { "pseudo.pseudo_total_additional_skill_gem_levels", null },
                // +# total to Level of Socketed Warcry Gems
                { "pseudo.pseudo_total_additional_warcry_gem_levels", null },
                // +# total to Level of Socketed Golem Gems
                { "pseudo.pseudo_total_additional_golem_gem_levels", null },
                // # Implicit Modifiers
                { "pseudo.pseudo_number_of_implicit_mods", null },
                // # Prefix Modifiers
                { "pseudo.pseudo_number_of_prefix_mods", null },
                // # Suffix Modifiers
                { "pseudo.pseudo_number_of_suffix_mods", null },
                // # Modifiers
                { "pseudo.pseudo_number_of_affix_mods", null },
                // # Crafted Prefix Modifiers
                { "pseudo.pseudo_number_of_crafted_prefix_mods", null },
                // # Crafted Suffix Modifiers
                { "pseudo.pseudo_number_of_crafted_suffix_mods", null },
                // # Crafted Modifiers
                { "pseudo.pseudo_number_of_crafted_mods", null },
                // # Empty Prefix Modifiers
                { "pseudo.pseudo_number_of_empty_prefix_mods", null },
                // # Empty Suffix Modifiers
                { "pseudo.pseudo_number_of_empty_suffix_mods", null },
                // # Empty Modifiers
                { "pseudo.pseudo_number_of_empty_affix_mods", null },
                // # Incubator Kills (Whispering)
                { "pseudo.pseudo_whispering_incubator_kills", null },
                // # Incubator Kills (Fine)
                { "pseudo.pseudo_fine_incubator_kills", null },
                // # Incubator Kills (Singular)
                { "pseudo.pseudo_singular_incubator_kills", null },
                // # Incubator Kills (Cartographer's)
                { "pseudo.pseudo_cartographers_incubator_kills", null },
                // # Incubator Kills (Otherwordly)
                { "pseudo.pseudo_otherworldly_incubator_kills", null },
                // # Incubator Kills (Abyssal)
                { "pseudo.pseudo_abyssal_incubator_kills", null },
                // # Incubator Kills (Fragmented)
                { "pseudo.pseudo_fragmented_incubator_kills", null },
                // # Incubator Kills (Skittering)
                { "pseudo.pseudo_skittering_incubator_kills", null },
                // # Incubator Kills (Infused)
                { "pseudo.pseudo_infused_incubator_kills", null },
                // # Incubator Kills (Fossilised)
                { "pseudo.pseudo_fossilised_incubator_kills", null },
                // # Incubator Kills (Decadent)
                { "pseudo.pseudo_decadent_incubator_kills", null },
                // # Incubator Kills (Diviner's)
                { "pseudo.pseudo_diviners_incubator_kills", null },
                // # Incubator Kills (Primal)
                { "pseudo.pseudo_primal_incubator_kills", null },
                // # Incubator Kills (Enchanted)
                { "pseudo.pseudo_enchanted_incubator_kills", null },
                // # Incubator Kills (Geomancer's)
                { "pseudo.pseudo_geomancers_incubator_kills", null },
                // # Incubator Kills (Ornate)
                { "pseudo.pseudo_ornate_incubator_kills", null },
                // # Incubator Kills (Time-Lost)
                { "pseudo.pseudo_timelost_incubator_kills", null },
                // # Incubator Kills (Celestial Armoursmith's)
                { "pseudo.pseudo_celestial_armoursmiths_incubator_kills", null },
                // # Incubator Kills (Celestial Blacksmith's)
                { "pseudo.pseudo_celestial_blacksmiths_incubator_kills", null },
                // # Incubator Kills (Celestial Jeweller's)
                { "pseudo.pseudo_celestial_jewellers_incubator_kills", null },
                // # Incubator Kills (Eldritch)
                { "pseudo.pseudo_eldritch_incubator_kills", null },
                // # Incubator Kills (Obscured)
                { "pseudo.pseudo_obscured_incubator_kills", null },
                // # Incubator Kills (Foreboding)
                { "pseudo.pseudo_foreboding_incubator_kills", null },
                // # Incubator Kills (Thaumaturge's)
                { "pseudo.pseudo_thaumaturges_incubator_kills", null },
                // # Incubator Kills (Mysterious)
                { "pseudo.pseudo_mysterious_incubator_kills", null },
                // # Incubator Kills (Gemcutter's)
                { "pseudo.pseudo_gemcutters_incubator_kills", null },
                // # Incubator Kills (Feral)
                { "pseudo.pseudo_feral_incubator_kills", null },
                // # Fractured Modifiers
                { "pseudo.pseudo_number_of_fractured_mods", null },
                // +#% Quality to Elemental Damage Modifiers
                { "pseudo.pseudo_jewellery_elemental_quality", null },
                // +#% Quality to Caster Modifiers
                { "pseudo.pseudo_jewellery_caster_quality", null },
                // +#% Quality to Attack Modifiers
                { "pseudo.pseudo_jewellery_attack_quality", null },
                // +#% Quality to Defence Modifiers
                { "pseudo.pseudo_jewellery_defense_quality", null },
                // +#% Quality to Life and Mana Modifiers
                { "pseudo.pseudo_jewellery_resource_quality", null },
                // +#% Quality to Resistance Modifiers
                { "pseudo.pseudo_jewellery_resistance_quality", null },
                // +#% Quality to Attribute Modifiers
                { "pseudo.pseudo_jewellery_attribute_quality", null },
            };
        }
    }
}
