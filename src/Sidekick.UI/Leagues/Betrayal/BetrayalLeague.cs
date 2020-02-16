using System.Collections.Generic;
using Sidekick.Localization.Leagues.Betrayal;

namespace Sidekick.UI.Leagues.Betrayal
{
    public class BetrayalLeague
    {
        public BetrayalLeague()
        {
            Agents = new List<BetrayalAgent>();

            Agents.Add(new BetrayalAgent(BetrayalResources.AislingName, "Aisling.png", RewardValue.Low)
            {
                Transportation = new BetrayalReward(BetrayalResources.AislingTransportaion, RewardValue.NoValue),
                Fortification = new BetrayalReward(BetrayalResources.AislingFortification, RewardValue.NoValue),
                Research = new BetrayalReward(BetrayalResources.AislingResearch, RewardValue.Low, BetrayalResources.AislingResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.AislingIntervention, RewardValue.NoValue)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.CameriaName, "Cameria.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.CameriaTransportation, RewardValue.Medium, BetrayalResources.CameriaTransportationTooltip),
                Fortification = new BetrayalReward(BetrayalResources.CameriaFortification, RewardValue.Medium),
                Research = new BetrayalReward(BetrayalResources.CameriaResearch, RewardValue.Medium),
                Intervention = new BetrayalReward(BetrayalResources.CameriaIntervention, RewardValue.High)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.ElreonName, "Elreon.png", RewardValue.NoValue)
            {
                Transportation = new BetrayalReward(BetrayalResources.ElreonTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(BetrayalResources.ElreonFortification, RewardValue.Low),
                Research = new BetrayalReward(BetrayalResources.ElreonResearch, RewardValue.Low),
                Intervention = new BetrayalReward(BetrayalResources.ElreonIntervention, RewardValue.Low)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.GraviciusName, "Gravicius.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.GraviciusTransportation, RewardValue.Medium),
                Fortification = new BetrayalReward(BetrayalResources.GraviciusFortification, RewardValue.Low),
                Research = new BetrayalReward(BetrayalResources.GraviciusResearch, RewardValue.NoValue),
                Intervention = new BetrayalReward(BetrayalResources.GraviciusIntervention, RewardValue.High)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.GuffName, "Guff.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(BetrayalResources.GuffTransportation, RewardValue.Medium, BetrayalResources.GuffTransportationTooltip),
                Fortification = new BetrayalReward(BetrayalResources.GuffFortification, RewardValue.Low, BetrayalResources.GuffFortificationTooltip),
                Research = new BetrayalReward(BetrayalResources.GuffResearch, RewardValue.Low),
                Intervention = new BetrayalReward(BetrayalResources.GuffIntervention, RewardValue.Low, BetrayalResources.GuffInterventionTooltip)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.HakuName, "Haku.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(BetrayalResources.HakuTransportation, RewardValue.NoValue),
                Fortification = new BetrayalReward(BetrayalResources.HakuFortification, RewardValue.NoValue),
                Research = new BetrayalReward(BetrayalResources.HakuResearch, RewardValue.NoValue),
                Intervention = new BetrayalReward(BetrayalResources.HakuIntervention, RewardValue.Medium)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.HillockName, "Hillock.png", RewardValue.Low)
            {
                Transportation = new BetrayalReward(BetrayalResources.HillockTransportation, RewardValue.Low, BetrayalResources.HillockTransportationTooltip),
                Fortification = new BetrayalReward(BetrayalResources.HillockFortification, RewardValue.Low, BetrayalResources.HillockFortificationTooltip),
                Research = new BetrayalReward(BetrayalResources.HillockResearch, RewardValue.Low, BetrayalResources.HillockResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.HillockIntervention, RewardValue.NoValue, BetrayalResources.HillockInterventionTooltip)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.ItThatFledName, "It_That_Fled.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.ItThatFledTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(BetrayalResources.ItThatFledFortification, RewardValue.Low),
                Research = new BetrayalReward(BetrayalResources.ItThatFledResearch, RewardValue.High, BetrayalResources.ItThatFledResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.ItThatFledIntervention, RewardValue.Low)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.JanusName, "Janus.png", RewardValue.Low)
            {
                Transportation = new BetrayalReward(BetrayalResources.JanusTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(BetrayalResources.JanusFortification, RewardValue.Low),
                Research = new BetrayalReward(BetrayalResources.JanusResearch, RewardValue.Low),
                Intervention = new BetrayalReward(BetrayalResources.JanusIntervention, RewardValue.Low)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.JorginName, "Jorgin.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(BetrayalResources.JorginTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(BetrayalResources.JorginFortification, RewardValue.Medium),
                Research = new BetrayalReward(BetrayalResources.JorginResearch, RewardValue.Medium, BetrayalResources.JorginResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.JorginIntervention, RewardValue.Low)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.KorellName, "Korell.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(BetrayalResources.KorellTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(BetrayalResources.KorellFortification, RewardValue.Medium),
                Research = new BetrayalReward(BetrayalResources.KorellResearch, RewardValue.Medium),
                Intervention = new BetrayalReward(BetrayalResources.KorellIntervention, RewardValue.Low)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.LeoName, "Leo.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(BetrayalResources.LeoTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(BetrayalResources.LeoFortification, RewardValue.Medium),
                Research = new BetrayalReward(BetrayalResources.LeoResearch, RewardValue.Medium, BetrayalResources.LeoResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.LeoIntervention, RewardValue.Low)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.RikerName, "Riker.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(BetrayalResources.RikerTransportation, RewardValue.Medium),
                Fortification = new BetrayalReward(BetrayalResources.RikerFortification, RewardValue.Low),
                Research = new BetrayalReward(BetrayalResources.RikerResearch, RewardValue.NoValue),
                Intervention = new BetrayalReward(BetrayalResources.RikerIntervention, RewardValue.Medium)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.RinName, "Rin.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.RinTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(BetrayalResources.RinFortification, RewardValue.Low),
                Research = new BetrayalReward(BetrayalResources.RinResearch, RewardValue.Low),
                Intervention = new BetrayalReward(BetrayalResources.RinIntervention, RewardValue.High)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.ToraName, "Tora.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.ToraTransportation, RewardValue.Medium, BetrayalResources.ToraTransportationTooltip),
                Fortification = new BetrayalReward(BetrayalResources.ToraFortification, RewardValue.Low, BetrayalResources.ToraFortificationTooltip),
                Research = new BetrayalReward(BetrayalResources.ToraResearch, RewardValue.High, BetrayalResources.ToraResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.ToraIntervention, RewardValue.Low)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.VaganName, "Vagan.png", RewardValue.Medium)
            {
                Transportation = new BetrayalReward(BetrayalResources.VaganTransportation, RewardValue.Medium),
                Fortification = new BetrayalReward(BetrayalResources.VaganFortification, RewardValue.Medium),
                Research = new BetrayalReward(BetrayalResources.VaganResearch, RewardValue.Medium),
                Intervention = new BetrayalReward(BetrayalResources.VaganIntervention, RewardValue.Medium)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.VoriciName, "Vorici.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.VoriciTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(BetrayalResources.VoriciFortification, RewardValue.Medium),
                Research = new BetrayalReward(BetrayalResources.VoriciResearch, RewardValue.High, BetrayalResources.VoriciResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.VoriciIntervention, RewardValue.Low)
            });
        }

        public List<BetrayalAgent> Agents { get; private set; }
    }
}
