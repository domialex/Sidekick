using System;
using MediatR;

namespace Sidekick.Domain.App.Commands
{
    /// <summary>
    /// Opens the specified Uri in a web browser
    /// </summary>
    public class OpenBrowserCommand : ICommand
    {
        public OpenBrowserCommand(Uri uri)
        {
            Uri = uri;
        }

        public Uri Uri { get; }
    }
}
