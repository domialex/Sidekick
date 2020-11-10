namespace Sidekick.Presentation.Wpf.Cheatsheets.Incursion
{
    public class IncursionRoomTier
    {
        public IncursionRoomTier(string name, RewardValue value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public RewardValue Value { get; set; }

        public string Color => Value.GetColor();
    }
}
