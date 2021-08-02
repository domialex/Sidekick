namespace Sidekick.Modules.Cheatsheets.Heist
{
    public class RewardModel
    {
        public RewardModel(string reward, RewardValue value, string image)
        {
            Reward = reward;
            Value = value;
            Image = image;
        }

        public string Reward { get; set; }

        public string Image { get; set; }

        public RewardValue Value { get; set; }
    }
}
