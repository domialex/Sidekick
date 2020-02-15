namespace Sidekick.UI.Leagues.Betrayal
{
    public class BetrayalAgent
    {
        public BetrayalAgent(string name, RewardValue value)
        {
            AgentName = name;
            AgentValue = value;
        }

        public string AgentName { get; set; }

        public RewardValue AgentValue { get; set; }

        public BetrayalReward Transportation { get; set; }

        public BetrayalReward Fortification { get; set; }

        public BetrayalReward Research { get; set; }

        public BetrayalReward Intervention { get; set; }
    }
}
