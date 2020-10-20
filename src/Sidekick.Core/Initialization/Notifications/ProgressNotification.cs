namespace Sidekick.Core.Initialization.Notifications
{
    public class ProgressNotification
    {
        public string Title { get; set; }
        public int TotalPercentage { get; set; }
        public string StepTitle { get; set; }
        public int StepPercentage { get; set; }
    }
}
