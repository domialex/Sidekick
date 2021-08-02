namespace Sidekick.Modules.Cheatsheets.Heist
{
    public class JobModel
    {
        public JobModel(string name, string image, RewardModel[] rewards, AllyModel[] allies)
        {
            JobName = name;
            Image = image;
            Rewards = rewards;
            Allies = allies;
        }

        public string JobName { get; set; }

        public string Image { get; set; }

        public RewardModel[] Rewards { get; set; }

        public AllyModel[] Allies { get; set; }
    }
}
