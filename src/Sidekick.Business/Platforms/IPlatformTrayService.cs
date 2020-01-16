namespace Sidekick.Business.Platforms
{
    public interface IPlatformTrayService
    {
        void UpdateLeagues();
        void SendNotification(string title, string text);
    }
}
