namespace Sidekick.Presentation.Cheatsheets.Heist
{
    public class HeistAlly
    {
        public HeistAlly(string name, short maxLevel, string image)
        {
            Name = name;
            MaxLevel = maxLevel;
            Image = image;
        }

        public string Name { get; set; }

        public short MaxLevel { get; set; }

        public string Image { get; set; }

        public string Text
        {
            get
            {
                return $"{Name} (Lv {MaxLevel})";
            }
        }
    }
}
