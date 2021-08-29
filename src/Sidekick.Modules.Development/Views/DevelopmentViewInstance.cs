using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Sidekick.Common.Blazor.Views;

namespace Sidekick.Development.Views
{
    public class DevelopmentViewInstance : IViewInstance
    {
        private readonly NavigationManager navigationManager;

        public DevelopmentViewInstance(NavigationManager navigationManager)
        {
            this.navigationManager = navigationManager;
        }

        public virtual Task Close()
        {
            navigationManager.NavigateTo("/");
            return Task.CompletedTask;
        }

        public virtual Task Maximize()
        {
            return Task.CompletedTask;
        }

        public virtual Task Minimize()
        {
            return Task.CompletedTask;
        }

        public string Title { get; private set; }

        public virtual void SetTitle(string title)
        {
            Title = title;
        }
    }
}
