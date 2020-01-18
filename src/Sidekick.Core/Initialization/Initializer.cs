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
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger logger;

        public Initializer(IServiceProvider serviceProvider,
            ILogger logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;

            resetServices = typeof(IOnReset).GetImplementedInterface();
            beforeInitServices = typeof(IOnBeforeInit).GetImplementedInterface();
            initServices = typeof(IOnInit).GetImplementedInterface();
            afterInitServices = typeof(IOnAfterInit).GetImplementedInterface();
        }

        private List<Type> resetServices { get; set; }
        private List<Type> beforeInitServices { get; set; }
        private List<Type> initServices { get; set; }
        private List<Type> afterInitServices { get; set; }
        public bool IsReady { get; private set; }

        private List<Type> GetServiceInterfaces(List<Type> implementations)
        {
            var interfaces = new List<Type>();
            foreach (var implementation in implementations)
            {
                var test = serviceProvider.GetService(implementation.GetInterfaces()[0]);
                var implementationInterfaces = implementation
                    .GetInterfaces()
                    .Where(x => serviceProvider.GetService(x) != null);
                foreach (var @interface in implementationInterfaces)
                {
                    interfaces.Add(@interface);
                }
            }
            return interfaces;
        }

        public async Task Initialize()
        {
            IsReady = false;
            await OnReset();
            await OnBeforeInit();
            await OnInit();
            await OnAfterInit();
            IsReady = true;
        }

        public async Task Reset()
        {
            IsReady = false;
            await OnReset();
        }

        private async Task OnReset()
        {
            var tasks = new List<Task>(resetServices.Count);
            foreach (var service in GetServiceInterfaces(resetServices))
            {
                tasks.Add(((IOnReset)serviceProvider.GetService(service)).OnReset());
            }
            await Task.WhenAll(tasks);
        }

        private async Task OnBeforeInit()
        {
            var tasks = new List<Task>(beforeInitServices.Count);
            foreach (var service in GetServiceInterfaces(beforeInitServices))
            {
                tasks.Add(((IOnBeforeInit)serviceProvider.GetService(service)).OnBeforeInit());
            }
            await Task.WhenAll(tasks);

        }

        private async Task OnInit()
        {
            var tasks = new List<Task>(initServices.Count);
            foreach (var service in GetServiceInterfaces(initServices))
            {
                tasks.Add(((IOnInit)serviceProvider.GetService(service)).OnInit());
            }
            await Task.WhenAll(tasks);
        }

        private async Task OnAfterInit()
        {
            var tasks = new List<Task>(afterInitServices.Count);
            foreach (var service in GetServiceInterfaces(afterInitServices))
            {
                tasks.Add(((IOnAfterInit)serviceProvider.GetService(service)).OnAfterInit());
            }
            await Task.WhenAll(tasks);
        }
    }
}
