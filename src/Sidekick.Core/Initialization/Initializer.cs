using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

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

        #region Progress
        private int ResetCount { get; set; }
        private int ResetCompleted { get; set; }
        private int ResetPercentage => ResetCount == 0 ? 0 : ResetCompleted * 100 / ResetCount;
        private int BeforeInitCount { get; set; }
        private int BeforeInitCompleted { get; set; }
        private int BeforeInitPercentage => BeforeInitCount == 0 ? 0 : BeforeInitCompleted * 100 / BeforeInitCount;
        private int InitCount { get; set; }
        private int InitCompleted { get; set; }
        private int InitPercentage => InitCount == 0 ? 0 : InitCompleted * 100 / InitCount;
        private int AfterInitCount { get; set; }
        private int AfterInitCompleted { get; set; }
        private int AfterInitPercentage => AfterInitCount == 0 ? 0 : AfterInitCompleted * 100 / AfterInitCount;

        private int TotalPercentage =>
            BeforeInitCount + InitCount + AfterInitCount == 0 ? 0 :
            (BeforeInitCompleted + InitCompleted + AfterInitCompleted) * 100 / (BeforeInitCount + InitCount + AfterInitCount);

        public event Action<ProgressEventArgs> OnProgress;

        public void ReportProgress(ProgressTypeEnum progressType, string serviceName, string message)
        {
            var percentage = TotalPercentage;

            switch (progressType)
            {
                case ProgressTypeEnum.Reset: percentage = ResetPercentage; break;
                case ProgressTypeEnum.BeforeInit: percentage = BeforeInitPercentage; break;
                case ProgressTypeEnum.Init: percentage = InitPercentage; break;
                case ProgressTypeEnum.AfterInit: percentage = AfterInitPercentage; break;
            }

            OnProgress?.Invoke(new ProgressEventArgs()
            {
                ServiceName = serviceName,
                Message = message,
                Percentage = percentage,
                TotalPercentage = TotalPercentage,
                Type = progressType,
            });
        }

        #endregion

        public async Task Initialize()
        {
            // Doing this here, injecting dependencies in constructor causes dependency loops
            resetServices = GetImplementations(resetServices);
            beforeInitServices = GetImplementations(beforeInitServices);
            initServices = GetImplementations(initServices);
            afterInitServices = GetImplementations(afterInitServices);

            ResetCount = resetServices.Count;
            BeforeInitCount = beforeInitServices.Count;
            InitCount = initServices.Count;
            AfterInitCount = afterInitServices.Count;

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
            await Task.WhenAll(resetServices.Select(s => Task.Run(async () =>
            {
                ReportProgress(ProgressTypeEnum.Reset, s.GetType().Name, "Initializer - Start Reset");
                await s.OnReset();
                ResetCompleted++;
                ReportProgress(ProgressTypeEnum.Reset, s.GetType().Name, "Initializer - End Reset");
            })));
        }

        private async Task OnBeforeInit()
        {
            foreach (var s in beforeInitServices)
            {
                ReportProgress(ProgressTypeEnum.BeforeInit, s.GetType().Name, "Initializer - Start Before Init");
                await s.OnBeforeInit();
                BeforeInitCompleted++;
                ReportProgress(ProgressTypeEnum.BeforeInit, s.GetType().Name, "Initializer - End Before Init");
            }
        }

        private async Task OnInit()
        {
            foreach (var s in initServices)
            {
                ReportProgress(ProgressTypeEnum.Init, s.GetType().Name, "Initializer - Start Init");
                await s.OnInit();
                InitCompleted++;
                ReportProgress(ProgressTypeEnum.Init, s.GetType().Name, "Initializer - End Init");
            }
        }

        private async Task OnAfterInit()
        {
            foreach (var s in afterInitServices)
            {
                ReportProgress(ProgressTypeEnum.AfterInit, s.GetType().Name, "Initializer - Start After Init");
                await s.OnAfterInit();
                AfterInitCompleted++;
                ReportProgress(ProgressTypeEnum.AfterInit, s.GetType().Name, "Initializer - End After Init");
            }
        }
    }
}
