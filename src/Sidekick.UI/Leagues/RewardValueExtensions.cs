namespace Sidekick.UI.Leagues
{
    public static class RewardValueExtensions
    {
        public static string GetColor(this RewardValue value)
        {
            return value switch
            {
                RewardValue.High => "#469408",
                RewardValue.Medium => "#ffc107",
                RewardValue.Low => "#D9831F",
                _ => "#D9230F",
            };
        }
    }
}
