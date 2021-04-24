using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.App.Commands;

namespace Sidekick.Application.App
{
    public class OpenBrowserHandler : ICommandHandler<OpenBrowserCommand>
    {
        private readonly ILogger logger;

        public OpenBrowserHandler(ILogger<OpenBrowserHandler> logger)
        {
            this.logger = logger;
        }

        public Task<Unit> Handle(OpenBrowserCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Opening in browser: {uri}", request.Uri.AbsoluteUri);
            var psi = new ProcessStartInfo
            {
                FileName = request.Uri.AbsoluteUri,
                UseShellExecute = true
            };
            Process.Start(psi);
            return Unit.Task;
        }
    }
}
