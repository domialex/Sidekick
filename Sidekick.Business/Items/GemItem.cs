namespace Sidekick.Business.Items
{
    public class GemItem : Item
    {
        public string Level { get; set; }
        public string Quality { get; set; }
        public bool IsVaalVersion { get; set; }
        public int ExperiencePercent { get; set; } // percent towards next level
    }
}
