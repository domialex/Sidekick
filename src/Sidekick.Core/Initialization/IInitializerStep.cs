using System;
using System.Threading.Tasks;

namespace Sidekick.Core.Initialization
{
    internal interface IInitializerStep
    {
        string Name { get; }
        int Count { get; }
        int Completed { get; set; }
        int Percentage { get; }
        Task Run(bool isFirstRun, Action<string> onStartHandler, Action<string> onEndHandler);
    }
}
