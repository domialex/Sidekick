namespace Sidekick.UI.Leagues.Betrayal
{
    public class BetrayalReward
    {
        public BetrayalReward(string reward, RewardValue value, string tooltip = "")
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
