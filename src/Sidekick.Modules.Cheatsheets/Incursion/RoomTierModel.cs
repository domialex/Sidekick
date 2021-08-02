namespace Sidekick.Modules.Cheatsheets.Incursion
{
    public class RoomTierModel
    {
        public RoomTierModel(string name, RewardValue value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }

        public RewardValue Value { get; set; }
    }
}
