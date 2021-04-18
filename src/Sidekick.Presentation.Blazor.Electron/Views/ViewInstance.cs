using System.Linq;
using System.Threading.Tasks;
using Sidekick.Domain.Views;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    public class ViewInstance : IViewInstance
    {
        private readonly InternalViewInstance view;
        private readonly IViewPreferenceRepository viewPreferenceRepository;

        public ViewInstance(ViewLocator locator, IViewPreferenceRepository viewPreferenceRepository)
        {
            view = locator.Views.Last();
            this.viewPreferenceRepository = viewPreferenceRepository;
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

        public string Title { get; private set; } = "Sidekick";

        public void SetTitle(string title)
        {
            Title = title;
            // Make sure the title is always Sidekick. For keybind management we are watching for this value.
            // view.Browser.SetTitle(title ?? "Sidekick");
        }
    }
}
