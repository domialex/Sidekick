namespace Sidekick.Domain.Cheatsheets.Delve
{
    public class DelveFossil
    {
        public DelveFossil(string name, RewardValue value, bool behindFracturedWall = false)
        {
            Name = name;
            Value = value;
            BehindFracturedWall = behindFracturedWall;
        }

        public string Name { get; set; }
        public RewardValue Value { get; set; }
        public bool BehindFracturedWall { get; set; }
    }
}
