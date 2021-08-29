using System.Linq;
using System.Threading.Tasks;
using ElectronNET.API.Entities;
using Sidekick.Common.Blazor.Views;
using Sidekick.Common.Cache;

namespace Sidekick.Presentation.Blazor.Electron.Views
{
    public class ViewInstance : IViewInstance
    {
        private readonly InternalViewInstance view;
        private readonly ICacheProvider cacheProvider;

        public ViewInstance(
            ViewLocator locator,
            ICacheProvider cacheProvider)
        {
            view = locator.Views.Last();
            this.cacheProvider = cacheProvider;
        }

        public async Task Initialize(string title, int width = 768, int height = 600, bool isOverlay = false, bool isModal = false, bool closeOnBlur = false)
        {
            Title = title;
            MinWidth = width;
            MinHeight = height;

            view.Browser.SetSize(width, height);
            view.Browser.SetMinimumSize(width, height);

            var preferences = await cacheProvider.Get<ViewPreferences>($"view_preference_{view.Key}");
            if (preferences != null)
            {
                view.Browser.SetSize(preferences.Width, preferences.Height);
            }

            if (isOverlay)
            {
                view.Browser.SetAlwaysOnTop(true);
                view.Browser.SetMaximizable(false);
                view.Browser.SetMinimizable(false);
                view.Browser.SetSkipTaskbar(true);
                view.Browser.SetAlwaysOnTop(true, OnTopLevel.screenSaver);
                view.Browser.ShowInactive();
            }
            else if (isModal)
            {
                view.Browser.SetMaximizable(false);
                view.Browser.SetMinimizable(false);
                view.Browser.SetResizable(false);
                view.Browser.ShowInactive();
            }
            else
            {
                view.Browser.Show();
            }
        }

        public string Title { get; private set; } = "Sidekick";

        public int MinWidth { get; set; }

        public int MinHeight { get; set; }

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
                var preferences = await cacheProvider.Get<ViewPreferences>($"view_preference_{view.Key}");
                if (preferences != null)
                {
                    MinWidth = preferences.Width;
                    MinHeight = preferences.Height;
                }

                view.Browser.SetSize(MinWidth, MinHeight);
                view.Browser.Center();
            }
        }

        public Task Close()
        {
            view.Browser.Close();
            return Task.CompletedTask;
        }
    }
}
