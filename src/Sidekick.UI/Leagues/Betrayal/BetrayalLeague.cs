using System.Collections.Generic;
using Sidekick.Localization.Leagues.Betrayal;

namespace Sidekick.UI.Leagues.Betrayal
{
    public class BetrayalLeague
    {
        public BetrayalLeague()
        {
            Agents = new List<BetrayalAgent>();

            Agents.Add(new BetrayalAgent(BetrayalResources.AislingName, "Aisling.png", RewardValue.Normal)
            {
                Transportation = new BetrayalReward(BetrayalResources.AislingTransportaion, RewardValue.Low),
                Fortification = new BetrayalReward(BetrayalResources.AislingFortification, RewardValue.Low),
                Research = new BetrayalReward(BetrayalResources.AislingResearch, RewardValue.Normal, BetrayalResources.AislingResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.AislingIntervention, RewardValue.Low)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.CameriaName, "Cameria.png", RewardValue.VeryHigh)
            {
                Transportation = new BetrayalReward(BetrayalResources.CameriaTransportation, RewardValue.High, BetrayalResources.CameriaTransportationTooltip),
                Fortification = new BetrayalReward(BetrayalResources.CameriaFortification, RewardValue.High),
                Research = new BetrayalReward(BetrayalResources.CameriaResearch, RewardValue.High),
                Intervention = new BetrayalReward(BetrayalResources.CameriaIntervention, RewardValue.VeryHigh)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.ElreonName, "Elreon.png", RewardValue.Low)
            {
                Transportation = new BetrayalReward(BetrayalResources.ElreonTransportation, RewardValue.Normal),
                Fortification = new BetrayalReward(BetrayalResources.ElreonFortification, RewardValue.Normal),
                Research = new BetrayalReward(BetrayalResources.ElreonResearch, RewardValue.Normal),
                Intervention = new BetrayalReward(BetrayalResources.ElreonIntervention, RewardValue.Normal)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.GraviciusName, "Gravicius.png", RewardValue.VeryHigh)
            {
                Transportation = new BetrayalReward(BetrayalResources.GraviciusTransportation, RewardValue.High),
                Fortification = new BetrayalReward(BetrayalResources.GraviciusFortification, RewardValue.Normal),
                Research = new BetrayalReward(BetrayalResources.GraviciusResearch, RewardValue.Low),
                Intervention = new BetrayalReward(BetrayalResources.GraviciusIntervention, RewardValue.VeryHigh)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.GuffName, "Guff.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.GuffTransportation, RewardValue.High, BetrayalResources.GuffTransportationTooltip),
                Fortification = new BetrayalReward(BetrayalResources.GuffFortification, RewardValue.Normal, BetrayalResources.GuffFortificationTooltip),
                Research = new BetrayalReward(BetrayalResources.GuffResearch, RewardValue.Normal),
                Intervention = new BetrayalReward(BetrayalResources.GuffIntervention, RewardValue.Normal, BetrayalResources.GuffInterventionTooltip)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.HakuName, "Haku.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.HakuTransportation, RewardValue.Low),
                Fortification = new BetrayalReward(BetrayalResources.HakuFortification, RewardValue.Low),
                Research = new BetrayalReward(BetrayalResources.HakuResearch, RewardValue.Low),
                Intervention = new BetrayalReward(BetrayalResources.HakuIntervention, RewardValue.High)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.HillockName, "Hillock.png", RewardValue.Normal)
            {
                Transportation = new BetrayalReward(BetrayalResources.HillockTransportation, RewardValue.Normal, BetrayalResources.HillockTransportationTooltip),
                Fortification = new BetrayalReward(BetrayalResources.HillockFortification, RewardValue.Normal, BetrayalResources.HillockFortificationTooltip),
                Research = new BetrayalReward(BetrayalResources.HillockResearch, RewardValue.Normal, BetrayalResources.HillockResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.HillockIntervention, RewardValue.Low, BetrayalResources.HillockInterventionTooltip)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.ItThatFledName, "It_That_Fled.png", RewardValue.VeryHigh)
            {
                Transportation = new BetrayalReward(BetrayalResources.ItThatFledTransportation, RewardValue.Normal),
                Fortification = new BetrayalReward(BetrayalResources.ItThatFledFortification, RewardValue.Normal),
                Research = new BetrayalReward(BetrayalResources.ItThatFledResearch, RewardValue.VeryHigh, BetrayalResources.ItThatFledResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.ItThatFledIntervention, RewardValue.Normal)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.JanusName, "Janus.png", RewardValue.Normal)
            {
                Transportation = new BetrayalReward(BetrayalResources.JanusTransportation, RewardValue.Normal),
                Fortification = new BetrayalReward(BetrayalResources.JanusFortification, RewardValue.Normal),
                Research = new BetrayalReward(BetrayalResources.JanusResearch, RewardValue.Normal),
                Intervention = new BetrayalReward(BetrayalResources.JanusIntervention, RewardValue.Normal)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.JorginName, "Jorgin.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.JorginTransportation, RewardValue.Normal),
                Fortification = new BetrayalReward(BetrayalResources.JorginFortification, RewardValue.High),
                Research = new BetrayalReward(BetrayalResources.JorginResearch, RewardValue.High, BetrayalResources.JorginResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.JorginIntervention, RewardValue.Normal)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.KorellName, "Korell.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.KorellTransportation, RewardValue.Normal),
                Fortification = new BetrayalReward(BetrayalResources.KorellFortification, RewardValue.High),
                Research = new BetrayalReward(BetrayalResources.KorellResearch, RewardValue.High),
                Intervention = new BetrayalReward(BetrayalResources.KorellIntervention, RewardValue.Normal)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.LeoName, "Leo.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.LeoTransportation, RewardValue.Normal),
                Fortification = new BetrayalReward(BetrayalResources.LeoFortification, RewardValue.High),
                Research = new BetrayalReward(BetrayalResources.LeoResearch, RewardValue.High, BetrayalResources.LeoResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.LeoIntervention, RewardValue.Normal)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.RikerName, "Riker.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.RikerTransportation, RewardValue.High),
                Fortification = new BetrayalReward(BetrayalResources.RikerFortification, RewardValue.Normal),
                Research = new BetrayalReward(BetrayalResources.RikerResearch, RewardValue.Low),
                Intervention = new BetrayalReward(BetrayalResources.RikerIntervention, RewardValue.High)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.RinName, "Rin.png", RewardValue.VeryHigh)
            {
                Transportation = new BetrayalReward(BetrayalResources.RinTransportation, RewardValue.Normal),
                Fortification = new BetrayalReward(BetrayalResources.RinFortification, RewardValue.Normal),
                Research = new BetrayalReward(BetrayalResources.RinResearch, RewardValue.Normal),
                Intervention = new BetrayalReward(BetrayalResources.RinIntervention, RewardValue.VeryHigh)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.ToraName, "Tora.png", RewardValue.VeryHigh)
            {
                Transportation = new BetrayalReward(BetrayalResources.ToraTransportation, RewardValue.High, BetrayalResources.ToraTransportationTooltip),
                Fortification = new BetrayalReward(BetrayalResources.ToraFortification, RewardValue.Normal, BetrayalResources.ToraFortificationTooltip),
                Research = new BetrayalReward(BetrayalResources.ToraResearch, RewardValue.VeryHigh, BetrayalResources.ToraResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.ToraIntervention, RewardValue.Normal)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.VaganName, "Vagan.png", RewardValue.High)
            {
                Transportation = new BetrayalReward(BetrayalResources.VaganTransportation, RewardValue.High),
                Fortification = new BetrayalReward(BetrayalResources.VaganFortification, RewardValue.High),
                Research = new BetrayalReward(BetrayalResources.VaganResearch, RewardValue.High),
                Intervention = new BetrayalReward(BetrayalResources.VaganIntervention, RewardValue.High)
            });

            Agents.Add(new BetrayalAgent(BetrayalResources.VoriciName, "Vorici.png", RewardValue.VeryHigh)
            {
                Transportation = new BetrayalReward(BetrayalResources.VoriciTransportation, RewardValue.Normal),
                Fortification = new BetrayalReward(BetrayalResources.VoriciFortification, RewardValue.High),
                Research = new BetrayalReward(BetrayalResources.VoriciResearch, RewardValue.VeryHigh, BetrayalResources.VoriciResearchTooltip),
                Intervention = new BetrayalReward(BetrayalResources.VoriciIntervention, RewardValue.Normal)
            });
        }

        public List<BetrayalAgent> Agents { get; private set; }
    }
}
