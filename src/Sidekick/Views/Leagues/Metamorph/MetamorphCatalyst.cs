namespace Sidekick.Views.Leagues.Metamorph
{
    public class MetamorphCatalyst
    {
        public MetamorphCatalyst(string name, string description, RewardValue value, string image)
        {
            Name = name;
            Description = description;
            Value = value;
            Image = image;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public RewardValue Value { get; set; }

        public string Image { get; set; }

        public string Color => Value.GetColor();
    }
}
