namespace Sidekick.Domain.Cheatsheets.Betrayal
{
    public class BetrayalAgent
    {
        public BetrayalAgent(string name, string image, RewardValue value)
        {
            Name = name;
            Image = image;
            Value = value;
        }

        public string Name { get; set; }

        public string Image { get; set; }

        public RewardValue Value { get; set; }

        public BetrayalReward Transportation { get; set; }

        public BetrayalReward Fortification { get; set; }

        public BetrayalReward Research { get; set; }

        public BetrayalReward Intervention { get; set; }
    }
}
