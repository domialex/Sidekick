using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cheatsheets;
using Sidekick.Domain.Cheatsheets.Betrayal;
using Sidekick.Localization.Cheatsheets;

namespace Sidekick.Presentation.Cheatsheets.Betrayal
{
    public class GetBetrayalCheatsheetHandler : IQueryHandler<GetBetrayalCheatsheetQuery, BetrayalLeague>
    {
        private readonly BetrayalResources resources;

        public GetBetrayalCheatsheetHandler(
            BetrayalResources resources)
        {
            this.resources = resources;
        }

        public Task<BetrayalLeague> Handle(GetBetrayalCheatsheetQuery request, CancellationToken cancellationToken)
        {
            var cheatsheet = new BetrayalLeague();

            cheatsheet.Agents.Add(new BetrayalAgent(resources.AislingName, "Aisling.png", RewardValue.Low)
            {
                Transportation = new BetrayalReward(resources.AislingTransportation, RewardValue.NoValue),
                Fortification = new BetrayalReward(resources.AislingFortification, RewardValue.NoValue),
                Research = new BetrayalReward(resources.AislingResearch, RewardValue.Low, resources.AislingResearchTooltip),
                Intervention = new BetrayalReward(resources.AislingIntervention, RewardValue.Low)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.CameriaName, "Cameria.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(resources.CameriaTransportation, RewardValue.Medium, resources.CameriaTransportationTooltip),
                Fortification = new BetrayalReward(resources.CameriaFortification, RewardValue.Medium),
                Research = new BetrayalReward(resources.CameriaResearch, RewardValue.Medium),
                Intervention = new BetrayalReward(resources.CameriaIntervention, RewardValue.High)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.ElreonName, "Elreon.png", RewardValue.NoValue)
            {
                Transportation = new BetrayalReward(resources.ElreonTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(resources.ElreonFortification, RewardValue.Low),
                Research = new BetrayalReward(resources.ElreonResearch, RewardValue.Low),
                Intervention = new BetrayalReward(resources.ElreonIntervention, RewardValue.Low)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.GraviciusName, "Gravicius.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(resources.GraviciusTransportation, RewardValue.Medium),
                Fortification = new BetrayalReward(resources.GraviciusFortification, RewardValue.Low),
                Research = new BetrayalReward(resources.GraviciusResearch, RewardValue.NoValue),
                Intervention = new BetrayalReward(resources.GraviciusIntervention, RewardValue.High)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.GuffName, "Guff.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(resources.GuffTransportation, RewardValue.Medium, resources.GuffTransportationTooltip),
                Fortification = new BetrayalReward(resources.GuffFortification, RewardValue.Low, resources.GuffFortificationTooltip),
                Research = new BetrayalReward(resources.GuffResearch, RewardValue.Low),
                Intervention = new BetrayalReward(resources.GuffIntervention, RewardValue.Low, resources.GuffInterventionTooltip)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.HakuName, "Haku.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(resources.HakuTransportation, RewardValue.NoValue),
                Fortification = new BetrayalReward(resources.HakuFortification, RewardValue.NoValue),
                Research = new BetrayalReward(resources.HakuResearch, RewardValue.NoValue),
                Intervention = new BetrayalReward(resources.HakuIntervention, RewardValue.Medium)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.HillockName, "Hillock.png", RewardValue.Low)
            {
                Transportation = new BetrayalReward(resources.HillockTransportation, RewardValue.Low, resources.HillockTransportationTooltip),
                Fortification = new BetrayalReward(resources.HillockFortification, RewardValue.Low, resources.HillockFortificationTooltip),
                Research = new BetrayalReward(resources.HillockResearch, RewardValue.Low, resources.HillockResearchTooltip),
                Intervention = new BetrayalReward(resources.HillockIntervention, RewardValue.NoValue, resources.HillockInterventionTooltip)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.ItThatFledName, "It_That_Fled.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(resources.ItThatFledTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(resources.ItThatFledFortification, RewardValue.Low),
                Research = new BetrayalReward(resources.ItThatFledResearch, RewardValue.High, resources.ItThatFledResearchTooltip),
                Intervention = new BetrayalReward(resources.ItThatFledIntervention, RewardValue.Low)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.JanusName, "Janus.png", RewardValue.Low)
            {
                Transportation = new BetrayalReward(resources.JanusTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(resources.JanusFortification, RewardValue.Low),
                Research = new BetrayalReward(resources.JanusResearch, RewardValue.Low),
                Intervention = new BetrayalReward(resources.JanusIntervention, RewardValue.Low)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.JorginName, "Jorgin.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(resources.JorginTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(resources.JorginFortification, RewardValue.Medium),
                Research = new BetrayalReward(resources.JorginResearch, RewardValue.Medium, resources.JorginResearchTooltip),
                Intervention = new BetrayalReward(resources.JorginIntervention, RewardValue.Low)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.KorellName, "Korell.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(resources.KorellTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(resources.KorellFortification, RewardValue.Medium),
                Research = new BetrayalReward(resources.KorellResearch, RewardValue.Medium),
                Intervention = new BetrayalReward(resources.KorellIntervention, RewardValue.Low)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.LeoName, "Leo.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(resources.LeoTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(resources.LeoFortification, RewardValue.Medium),
                Research = new BetrayalReward(resources.LeoResearch, RewardValue.Medium, resources.LeoResearchTooltip),
                Intervention = new BetrayalReward(resources.LeoIntervention, RewardValue.Low)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.RikerName, "Riker.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(resources.RikerTransportation, RewardValue.Medium),
                Fortification = new BetrayalReward(resources.RikerFortification, RewardValue.Low),
                Research = new BetrayalReward(resources.RikerResearch, RewardValue.NoValue),
                Intervention = new BetrayalReward(resources.RikerIntervention, RewardValue.Medium)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.RinName, "Rin.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(resources.RinTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(resources.RinFortification, RewardValue.Low),
                Research = new BetrayalReward(resources.RinResearch, RewardValue.Low),
                Intervention = new BetrayalReward(resources.RinIntervention, RewardValue.High)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.ToraName, "Tora.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(resources.ToraTransportation, RewardValue.Medium, resources.ToraTransportationTooltip),
                Fortification = new BetrayalReward(resources.ToraFortification, RewardValue.Low, resources.ToraFortificationTooltip),
                Research = new BetrayalReward(resources.ToraResearch, RewardValue.High, resources.ToraResearchTooltip),
                Intervention = new BetrayalReward(resources.ToraIntervention, RewardValue.Low)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.VaganName, "Vagan.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(resources.VaganTransportation, RewardValue.Medium),
                Fortification = new BetrayalReward(resources.VaganFortification, RewardValue.Medium),
                Research = new BetrayalReward(resources.VaganResearch, RewardValue.Medium),
                Intervention = new BetrayalReward(resources.VaganIntervention, RewardValue.Medium)
            });

            cheatsheet.Agents.Add(new BetrayalAgent(resources.VoriciName, "Vorici.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(resources.VoriciTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(resources.VoriciFortification, RewardValue.Medium),
                Research = new BetrayalReward(resources.VoriciResearch, RewardValue.High, resources.VoriciResearchTooltip),
                Intervention = new BetrayalReward(resources.VoriciIntervention, RewardValue.Low)
            });

            return Task.FromResult(cheatsheet);
        }
    }
}
