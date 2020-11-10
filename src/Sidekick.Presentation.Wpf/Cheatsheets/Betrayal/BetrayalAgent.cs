namespace Sidekick.Presentation.Wpf.Cheatsheets.Betrayal
{
    public class BetrayalAgent
    {
        public BetrayalAgent(string name, string image, RewardValue value)
        {
            AgentName = name;
            Image = image;
            Value = value;
        }

        public string AgentName { get; set; }

        public string Image { get; set; }

        public RewardValue Value { get; set; }

        public string Color => Value.GetColor();

        public BetrayalReward Transportation { get; set; }

        public BetrayalReward Fortification { get; set; }

        public BetrayalReward Research { get; set; }

        public BetrayalReward Intervention { get; set; }
    }
}
