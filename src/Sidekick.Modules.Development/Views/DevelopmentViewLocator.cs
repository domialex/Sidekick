using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Sidekick.Common.Blazor.Views;

namespace Sidekick.Development.Views
{
    public class DevelopmentViewLocator : IViewLocator
    {
        private readonly NavigationManager navigationManager;

        public DevelopmentViewLocator(NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
        }

        public Task Open(string url)
        {
            navigationManager.NavigateTo(url);
            return Task.CompletedTask;
        }

        public void CloseAllOverlays()
        {
            // Do nothing
        }

        public bool IsOverlayOpened()
        {
            return false;
        }
    }
}
