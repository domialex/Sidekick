namespace Sidekick.Modules.Cheatsheets.Heist
{
    public class AllyModel
    {
        public AllyModel(string name, short maxLevel, string image)
        {
            Name = name;
            MaxLevel = maxLevel;
            Image = image;
        }

        public string Name { get; set; }

        public short MaxLevel { get; set; }

        public string Image { get; set; }
    }
}
