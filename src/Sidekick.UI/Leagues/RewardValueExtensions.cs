namespace Sidekick.UI.Leagues
{
    public static class RewardValueExtensions
    {
        public static string GetColor(this RewardValue value)
        {
            switch (value)
            {
                case RewardValue.High: return "#38B44A";
                case RewardValue.Medium: return "#EFB73E";
                case RewardValue.Low: return "#E95420";
                default: return "#DF382C";
            }
        }
    }
}
