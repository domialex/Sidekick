using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Serilog;

namespace Sidekick.Core.Initialization
{
    public class Initializer : IInitializer
    {
        private List<IOnReset> resetServices;
        private List<IOnBeforeInit> beforeInitServices;
        private List<IOnInit> initServices;
        private List<IOnAfterInit> afterInitServices;
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;
        private readonly IStringLocalizer<Initializer> localizer;

        public Initializer(ILogger logger,
            IServiceProvider serviceProvider,
            IStringLocalizer<Initializer> localizer)
        {
            this.logger = logger.ForContext(GetType());
            this.serviceProvider = serviceProvider;
            this.localizer = localizer;
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

        public void ReportProgress(InitializationSteps progressType, string serviceName, string message)
        {
            var args = new ProgressEventArgs()
            {
                Title = localizer[$"Type_{progressType}"],
                TotalPercentage = TotalPercentage,
                Step = progressType,
                StepTitle = serviceName,
                StepPercentage = TotalPercentage,
            };

            if (string.IsNullOrEmpty(args.Title))
            {
                args.Title = args.Step.ToString();
            }

            switch (progressType)
            {
                case InitializationSteps.Reset: args.StepPercentage = ResetPercentage; break;
                case InitializationSteps.BeforeInit: args.StepPercentage = BeforeInitPercentage; break;
                case InitializationSteps.Init: args.StepPercentage = InitPercentage; break;
                case InitializationSteps.AfterInit: args.StepPercentage = AfterInitPercentage; break;
            }

            if (TotalPercentage >= 100)
            {
                args.Title = localizer["Ready"];
                args.StepPercentage = 100;
                serviceName = string.Empty;
            }

            OnProgress?.Invoke(args);

            logger.Debug("{totalPercentage}% - {message} {serviceName}", TotalPercentage, message, serviceName);
        }

        #endregion

        private bool IsReady { get; set; } = false;
        private bool IsInitializing { get; set; } = false;

        public async Task Initialize()
        {
            if (IsInitializing) { return; }
            IsInitializing = true;

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

            ReportProgress(InitializationSteps.Other, nameof(Initializer), "Initializer - Start");
            OnReset();
            await OnBeforeInit();
            await OnInit();
            await OnAfterInit();

            IsInitializing = false;
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
                ReportProgress(InitializationSteps.Reset, s.GetType().Name, "Initializer - Start Reset");
                s.OnReset();
                ResetCompleted++;
                ReportProgress(InitializationSteps.Reset, s.GetType().Name, "Initializer - End Reset");
            }
        }

        private async Task OnBeforeInit()
        {
            foreach (var s in beforeInitServices)
            {
                var serviceName = s.GetType().Name;
                ReportProgress(InitializationSteps.BeforeInit, serviceName, "Initializer - Start Before Init");

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

                ReportProgress(InitializationSteps.BeforeInit, serviceName, "Initializer - End Before Init");
            }
        }

        private async Task OnInit()
        {
            foreach (var s in initServices)
            {
                var serviceName = s.GetType().Name;
                ReportProgress(InitializationSteps.Init, serviceName, "Initializer - Start Init");

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

                ReportProgress(InitializationSteps.Init, serviceName, "Initializer - End Init");
            }
        }

        private async Task OnAfterInit()
        {
            foreach (var s in afterInitServices)
            {
                var serviceName = s.GetType().Name;
                ReportProgress(InitializationSteps.AfterInit, serviceName, "Initializer - Start After Init");

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

                ReportProgress(InitializationSteps.AfterInit, serviceName, "Initializer - End After Init");
            }
        }
    }
}
