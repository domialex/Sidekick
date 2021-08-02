namespace Sidekick.Modules.Cheatsheets.Betrayal
{
    public class RewardModel
    {
        public RewardModel(string reward, RewardValue value, string tooltip = "")
        {
            Reward = reward;
            Value = value;
            Tooltip = tooltip;
        }

        public string Reward { get; set; }

        public string Tooltip { get; set; }

        public RewardValue Value { get; set; }
    }
}
