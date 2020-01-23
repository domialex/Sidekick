namespace Sidekick.Windows.TrayIcon.Models
{
    public class League : NotifyBase
    {
        private string id;
        private string name;
        private bool isCurrent;

        public string Id { get => id; set => NotifyProperty(ref id, value); }

        public string Name { get => name; set => NotifyProperty(ref name, value); }

        public bool IsCurrent { get => isCurrent; set => NotifyProperty(ref isCurrent, value); }
    }
}
