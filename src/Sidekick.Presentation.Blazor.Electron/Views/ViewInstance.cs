using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    public class ViewInstance : IViewInstance
    {
        private readonly InternalViewInstance view;
        private readonly IViewPreferenceRepository viewPreferenceRepository;
        private readonly IJSRuntime jSRuntime;

        public ViewInstance(ViewLocator locator, IViewPreferenceRepository viewPreferenceRepository, IJSRuntime jSRuntime)
        {
            view = locator.Views.Last();
            this.viewPreferenceRepository = viewPreferenceRepository;
            this.jSRuntime = jSRuntime;
        }

        public Task Minimize()
        {
            view.Browser.Minimize();
            return Task.CompletedTask;
        }

        public async Task Maximize()
        {
            if (!await view.Browser.IsMaximizedAsync())
            {
                view.Browser.Maximize();
            }
            else
            {
                var preferences = await viewPreferenceRepository.Get(view.View);
                if (preferences != null)
                {
                    view.Browser.SetSize(preferences.Width, preferences.Height);
                }
                else
                {
                    var (width, height) = ViewLocator.GetSize(view.View);
                    view.Browser.SetSize(width, height);
                }
                view.Browser.Center();
            }
        }

        public Task Close()
        {
            view.Browser.Close();
            return Task.CompletedTask;
        }

        public async Task SetTitle(string title)
        {
            await jSRuntime.InvokeVoidAsync("sidekickSetTitle", title);
            view.Browser.SetTitle(title);
        }
    }
}
