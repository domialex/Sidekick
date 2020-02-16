namespace Sidekick.UI.Leagues
{
    public static class RewardValueExtensions
    {
        public static string GetColor(this RewardValue value)
        {
            switch (value)
            {
                case RewardValue.VeryHigh: return "#469408";
                case RewardValue.High: return "#898989";
                case RewardValue.Normal: return "#D9831F";
                default: return "#D9230F";
            }
        }
    }
}
