using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace Sidekick.Presentation.Blazor.Mocks
{
    public class MockViewInstance : Sidekick.Mock.Views.MockViewInstance
    {
        private readonly IJSRuntime jSRuntime;

        public MockViewInstance(IJSRuntime jSRuntime)
        {
            this.jSRuntime = jSRuntime;
        }

        public override async Task SetTitle(string title)
        {
            await jSRuntime.InvokeVoidAsync("sidekickSetTitle", title);
        }
    }
}
