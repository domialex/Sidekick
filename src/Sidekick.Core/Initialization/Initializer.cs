using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Sidekick.Core.Initialization
{
    public class Initializer : IInitializer
    {
        public bool IsReady { get; private set; }

        private List<IOnReset> resetServices;
        private List<IOnBeforeInit> beforeInitServices;
        private List<IOnInit> initServices;
        private List<IOnAfterInit> afterInitServices;
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        public Initializer(ILogger logger,
            IServiceProvider serviceProvider)
        {
            this.logger = logger.ForContext(GetType());
            this.serviceProvider = serviceProvider;
        }

        #region Error handling
        public event Action<ErrorEventArgs> OnError;

        public void HandleError(Exception exception, string serviceName)
        {
            OnError?.Invoke(new ErrorEventArgs
            {
                ServiceName = serviceName,
                Message = exception.Message
            });
        }
        #endregion

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

            logger.Debug("{totalPercentage}% - {message} {serviceName}", TotalPercentage, message, serviceName);
        }

        #endregion

        public async Task Initialize()
        {
            // Doing this here, injecting dependencies in constructor causes dependency loops
            beforeInitServices = GetImplementations(beforeInitServices);
            initServices = GetImplementations(initServices);
            afterInitServices = GetImplementations(afterInitServices);
            resetServices = GetImplementations(resetServices);

            ResetCount = resetServices.Count;
            BeforeInitCount = beforeInitServices.Count;
            InitCount = initServices.Count;
            AfterInitCount = afterInitServices.Count;

            ResetCompleted = 0;
            BeforeInitCompleted = 0;
            InitCompleted = 0;
            AfterInitCompleted = 0;

            OnReset();
            await OnBeforeInit();
            await OnInit();
            await OnAfterInit();
            IsReady = true;
        }

        private List<T> GetImplementations<T>(List<T> implementationList)
        {
            return implementationList ?? serviceProvider.GetServices<T>().ToList();
        }

        private void OnReset()
        {
            foreach (var s in resetServices)
            {
                ReportProgress(ProgressTypeEnum.Reset, s.GetType().Name, "Initializer - Start Reset");
                s.OnReset();
                ResetCompleted++;
                ReportProgress(ProgressTypeEnum.Reset, s.GetType().Name, "Initializer - End Reset");
            }
        }

        private async Task OnBeforeInit()
        {
            foreach (var s in beforeInitServices)
            {
                var serviceName = s.GetType().Name;
                ReportProgress(ProgressTypeEnum.BeforeInit, serviceName, "Initializer - Start Before Init");

                try
                {
                    await s.OnBeforeInit();
                    BeforeInitCompleted++;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Fatal error during OnBeforeInit");
                    HandleError(ex, serviceName);
                }

                ReportProgress(ProgressTypeEnum.BeforeInit, serviceName, "Initializer - End Before Init");
            }
        }

        private async Task OnInit()
        {
            foreach (var s in initServices)
            {
                var serviceName = s.GetType().Name;
                ReportProgress(ProgressTypeEnum.Init, serviceName, "Initializer - Start Init");

                try
                {
                    await s.OnInit();
                    InitCompleted++;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Fatal error during OnInit");
                    HandleError(ex, serviceName);
                }

                ReportProgress(ProgressTypeEnum.Init, serviceName, "Initializer - End Init");
            }
        }

        private async Task OnAfterInit()
        {
            foreach (var s in afterInitServices)
            {
                var serviceName = s.GetType().Name;
                ReportProgress(ProgressTypeEnum.AfterInit, serviceName, "Initializer - Start After Init");

                try
                {
                    await s.OnAfterInit();
                    AfterInitCompleted++;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "Fatal error during OnAfterInit");
                    HandleError(ex, serviceName);
                }

                ReportProgress(ProgressTypeEnum.AfterInit, serviceName, "Initializer - End After Init");
            }
        }
    }
}
