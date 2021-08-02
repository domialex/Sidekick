namespace Sidekick.Modules.Cheatsheets.Betrayal
{
    public class AgentModel
    {
        public AgentModel(string name, string image, RewardValue value)
        {
            Name = name;
            Image = image;
            Value = value;
        }

        public string Name { get; set; }

        public string Image { get; set; }

        public RewardValue Value { get; set; }

        public RewardModel Transportation { get; set; }

        public RewardModel Fortification { get; set; }

        public RewardModel Research { get; set; }

        public RewardModel Intervention { get; set; }
    }
}
