using Sidekick.Core.Extensions;
using Sidekick.Core.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    public class InitializeService : IInitializeService
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger logger;

        public InitializeService(IServiceProvider serviceProvider,
            ILogger logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;

            resetServices = typeof(IOnReset).GetImplementedInterface();
            beforeInitializeServices = typeof(IOnBeforeInitialize).GetImplementedInterface();
            initializeServices = typeof(IOnInitialize).GetImplementedInterface();
            afterInitializeServices = typeof(IOnAfterInitialize).GetImplementedInterface();
        }

        private List<Type> resetServices { get; set; }
        private List<Type> beforeInitializeServices { get; set; }
        private List<Type> initializeServices { get; set; }
        private List<Type> afterInitializeServices { get; set; }
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
            try
            {
                await OnReset();
                await OnBeforeInitialize();
                await OnInitialize();
                await OnAfterInitialize();
                IsReady = true;
            }
            catch (Exception e)
            {
                logger.LogException(e);
            }
        }

        public async Task Reset()
        {
            IsReady = false;
            try
            {
                await OnReset();
            }
            catch (Exception e)
            {
                logger.LogException(e);
            }
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

        private async Task OnBeforeInitialize()
        {
            var tasks = new List<Task>(beforeInitializeServices.Count);
            foreach (var service in GetServiceInterfaces(beforeInitializeServices))
            {
                tasks.Add(((IOnBeforeInitialize)serviceProvider.GetService(service)).OnBeforeInitialize());
            }
            await Task.WhenAll(tasks);

        }

        private async Task OnInitialize()
        {
            var tasks = new List<Task>(initializeServices.Count);
            foreach (var service in GetServiceInterfaces(initializeServices))
            {
                tasks.Add(((IOnInitialize)serviceProvider.GetService(service)).OnInitialize());
            }
            await Task.WhenAll(tasks);
        }

        private async Task OnAfterInitialize()
        {
            var tasks = new List<Task>(afterInitializeServices.Count);
            foreach (var service in GetServiceInterfaces(afterInitializeServices))
            {
                tasks.Add(((IOnAfterInitialize)serviceProvider.GetService(service)).OnAfterInitialize());
            }
            await Task.WhenAll(tasks);
        }
    }
}
