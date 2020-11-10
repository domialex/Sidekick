namespace Sidekick.Presentation.Wpf.Cheatsheets.Heist
{
    public class HeistReward
    {
        public HeistReward(string reward, RewardValue value, string image)
        {
            Reward = reward;
            Value = value;
            Image = image;
        }

        public string Reward { get; set; }

        public string Image { get; set; }

        public RewardValue Value { get; set; }

        public string Color => Value.GetColor();
    }
}
