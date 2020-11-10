namespace Sidekick.Presentation.Wpf.Cheatsheets.Heist
{
    public class HeistJob
    {
        public HeistJob(string name, string image, HeistReward[] rewards, HeistAlly[] allies)
        {
            JobName = name;
            Image = image;
            Rewards = rewards;
            Allies = allies;
        }

        public string JobName { get; set; }

        public string Image { get; set; }

        public HeistReward[] Rewards { get; set; }

        public HeistAlly[] Allies { get; set; }
    }
}
