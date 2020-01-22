using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Extensions;
using Sidekick.Core.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    public class Initializer : IInitializer
    {
        public bool IsReady { get; private set; }

        private List<IOnReset> resetServices;
        private List<IOnBeforeInit> beforeInitServices;
        private List<IOnInit> initServices;
        private List<IOnAfterInit> afterInitServices;

        private readonly IServiceProvider serviceProvider;

        public Initializer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task Initialize()
        {
            // Doing this here, injecting dependencies in constructor causes dependency loops
            resetServices = GetImplementations(resetServices);
            beforeInitServices = GetImplementations(beforeInitServices);
            initServices = GetImplementations(initServices);
            afterInitServices = GetImplementations(afterInitServices);


            IsReady = false;
            await OnReset();
            await OnBeforeInit();
            await OnInit();
            await OnAfterInit();
            IsReady = true;
        }

        private List<T> GetImplementations<T>(List<T> implementationList)
        {
            return implementationList ?? serviceProvider.GetServices<T>().ToList();
        }

        public async Task Reset()
        {
            IsReady = false;
            await OnReset();
        }

        private async Task OnReset()
        {
            await Task.WhenAll(resetServices.Select(s => s.OnReset()));
        }

        private async Task OnBeforeInit()
        {
            await Task.WhenAll(beforeInitServices.Select(s => s.OnBeforeInit()));

        }

        private async Task OnInit()
        {
            await Task.WhenAll(initServices.Select(s => s.OnInit()));
        }

        private async Task OnAfterInit()
        {
            await Task.WhenAll(afterInitServices.Select(s => s.OnAfterInit()));
        }
    }
}
