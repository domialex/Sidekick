namespace Sidekick.UI.Leagues
{
    public static class RewardValueExtensions
    {
        public static string GetColor(this RewardValue value)
        {
            switch (value)
            {
                case RewardValue.High: return "#469408";
                case RewardValue.Medium: return "#ffc107";
                case RewardValue.Low: return "#D9831F";
                default: return "#D9230F";
            }
        }
    }
}
