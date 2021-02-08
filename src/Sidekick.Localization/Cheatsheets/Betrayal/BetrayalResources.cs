using Microsoft.Extensions.Localization;

namespace Sidekick.Localization.Cheatsheets.Betrayal
{
    public class BetrayalResources
    {
        private readonly IStringLocalizer<BetrayalResources> localizer;

        public BetrayalResources(IStringLocalizer<BetrayalResources> localizer)
        {
            this.localizer = localizer;
        }

        public string AislingFortification => localizer["AislingFortification"];
        public string AislingIntervention => localizer["AislingIntervention"];
        public string AislingName => localizer["AislingName"];
        public string AislingResearch => localizer["AislingResearch"];
        public string AislingResearchTooltip => localizer["AislingResearchTooltip"];
        public string AislingTransportation => localizer["AislingTransportation"];
        public string CameriaFortification => localizer["CameriaFortification"];
        public string CameriaIntervention => localizer["CameriaIntervention"];
        public string CameriaName => localizer["CameriaName"];
        public string CameriaResearch => localizer["CameriaResearch"];
        public string CameriaTransportation => localizer["CameriaTransportation"];
        public string CameriaTransportationTooltip => localizer["CameriaTransportationTooltip"];
        public string ElreonFortification => localizer["ElreonFortification"];
        public string ElreonIntervention => localizer["ElreonIntervention"];
        public string ElreonName => localizer["ElreonName"];
        public string ElreonResearch => localizer["ElreonResearch"];
        public string ElreonTransportation => localizer["ElreonTransportation"];
        public string GraviciusFortification => localizer["GraviciusFortification"];
        public string GraviciusIntervention => localizer["GraviciusIntervention"];
        public string GraviciusName => localizer["GraviciusName"];
        public string GraviciusResearch => localizer["GraviciusResearch"];
        public string GraviciusTransportation => localizer["GraviciusTransportation"];
        public string GuffFortification => localizer["GuffFortification"];
        public string GuffFortificationTooltip => localizer["GuffFortificationTooltip"];
        public string GuffIntervention => localizer["GuffIntervention"];
        public string GuffInterventionTooltip => localizer["GuffInterventionTooltip"];
        public string GuffName => localizer["GuffName"];
        public string GuffResearch => localizer["GuffResearch"];
        public string GuffResearchTooltip => localizer["GuffResearchTooltip"];
        public string GuffTransportation => localizer["GuffTransportation"];
        public string GuffTransportationTooltip => localizer["GuffTransportationTooltip"];
        public string HakuFortification => localizer["HakuFortification"];
        public string HakuIntervention => localizer["HakuIntervention"];
        public string HakuName => localizer["HakuName"];
        public string HakuResearch => localizer["HakuResearch"];
        public string HakuTransportation => localizer["HakuTransportation"];
        public string HillockFortification => localizer["HillockFortification"];
        public string HillockFortificationTooltip => localizer["HillockFortificationTooltip"];
        public string HillockIntervention => localizer["HillockIntervention"];
        public string HillockInterventionTooltip => localizer["HillockInterventionTooltip"];
        public string HillockName => localizer["HillockName"];
        public string HillockResearch => localizer["HillockResearch"];
        public string HillockResearchTooltip => localizer["HillockResearchTooltip"];
        public string HillockTransportation => localizer["HillockTransportation"];
        public string HillockTransportationTooltip => localizer["HillockTransportationTooltip"];
        public string ItThatFledFortification => localizer["ItThatFledFortification"];
        public string ItThatFledIntervention => localizer["ItThatFledIntervention"];
        public string ItThatFledName => localizer["ItThatFledName"];
        public string ItThatFledResearch => localizer["ItThatFledResearch"];
        public string ItThatFledResearchTooltip => localizer["ItThatFledResearchTooltip"];
        public string ItThatFledTransportation => localizer["ItThatFledTransportation"];
        public string JanusFortification => localizer["JanusFortification"];
        public string JanusIntervention => localizer["JanusIntervention"];
        public string JanusName => localizer["JanusName"];
        public string JanusResearch => localizer["JanusResearch"];
        public string JanusTransportation => localizer["JanusTransportation"];
        public string JorginFortification => localizer["JorginFortification"];
        public string JorginIntervention => localizer["JorginIntervention"];
        public string JorginName => localizer["JorginName"];
        public string JorginResearch => localizer["JorginResearch"];
        public string JorginResearchTooltip => localizer["JorginResearchTooltip"];
        public string JorginTransportation => localizer["JorginTransportation"];
        public string KorellFortification => localizer["KorellFortification"];
        public string KorellIntervention => localizer["KorellIntervention"];
        public string KorellName => localizer["KorellName"];
        public string KorellResearch => localizer["KorellResearch"];
        public string KorellTransportation => localizer["KorellTransportation"];
        public string LeoFortification => localizer["LeoFortification"];
        public string LeoIntervention => localizer["LeoIntervention"];
        public string LeoName => localizer["LeoName"];
        public string LeoResearch => localizer["LeoResearch"];
        public string LeoResearchTooltip => localizer["LeoResearchTooltip"];
        public string LeoTransportation => localizer["LeoTransportation"];
        public string RikerFortification => localizer["RikerFortification"];
        public string RikerIntervention => localizer["RikerIntervention"];
        public string RikerName => localizer["RikerName"];
        public string RikerResearch => localizer["RikerResearch"];
        public string RikerTransportation => localizer["RikerTransportation"];
        public string RinFortification => localizer["RinFortification"];
        public string RinIntervention => localizer["RinIntervention"];
        public string RinName => localizer["RinName"];
        public string RinResearch => localizer["RinResearch"];
        public string RinTransportation => localizer["RinTransportation"];
        public string ToraFortification => localizer["ToraFortification"];
        public string ToraFortificationTooltip => localizer["ToraFortificationTooltip"];
        public string ToraIntervention => localizer["ToraIntervention"];
        public string ToraName => localizer["ToraName"];
        public string ToraResearch => localizer["ToraResearch"];
        public string ToraResearchTooltip => localizer["ToraResearchTooltip"];
        public string ToraTransportation => localizer["ToraTransportation"];
        public string ToraTransportationTooltip => localizer["ToraTransportationTooltip"];
        public string TypeFortification => localizer["TypeFortification"];
        public string TypeIntervention => localizer["TypeIntervention"];
        public string TypeResearch => localizer["TypeResearch"];
        public string TypeTransportation => localizer["TypeTransportation"];
        public string VaganFortification => localizer["VaganFortification"];
        public string VaganIntervention => localizer["VaganIntervention"];
        public string VaganName => localizer["VaganName"];
        public string VaganResearch => localizer["VaganResearch"];
        public string VaganTransportation => localizer["VaganTransportation"];
        public string VoriciFortification => localizer["VoriciFortification"];
        public string VoriciIntervention => localizer["VoriciIntervention"];
        public string VoriciName => localizer["VoriciName"];
        public string VoriciResearch => localizer["VoriciResearch"];
        public string VoriciResearchTooltip => localizer["VoriciResearchTooltip"];
        public string VoriciTransportation => localizer["VoriciTransportation"];
    }
}
