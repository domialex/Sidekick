using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.App.Commands;

namespace Sidekick.Presentation.Blazor.About
{
    public class AboutViewModel : IDisposable
    {
        private readonly IMediator mediator;
        private readonly ILogger logger;
        public string VersionNumber { get; private set; }
        public string OperatingSystem { get; private set; }
        public string EnvironmentVersion { get; private set; }
        public string ProjectUrl { get; private set; } = "https://github.com/domialex/Sidekick";
        public string BugUrl { get; private set; } = "https://github.com/domialex/Sidekick/issues";
        public Dictionary<string, string> Contributors { get; private set; } = new Dictionary<string, string>()
        {
            { "domialex", "https://github.com/domialex" },
            { "leMicin", "https://github.com/leMicin" },
            { "Blinke", "https://github.com/Blinke" },
            { "Zalhera", "https://github.com/Zalhera" },
            { "cmos12345", "https://github.com/cmos12345" },
            { "kai-oswald", "https://github.com/kai-oswald" },
            { "Arisix", "https://github.com/Arisix" },
            { "dandrew-xx", "https://github.com/dandrew-xx" },
            { "pobiega", "https://github.com/pobiega" },
            { "lascoin", "https://github.com/lascoin" },
            { "vestakip", "https://github.com/vestakip" },
            { "PeteyPii", "https://github.com/PeteyPii" },
            { "mwardrop", "https://github.com/mwardrop" }
        };
        public Dictionary<string, string> Translators { get; private set; } = new Dictionary<string, string>()
        {
            { "Taiwanese Mandarin - Arisix", "https://github.com/Arisix" }

        };
        public Dictionary<string, string> ThirdParties { get; private set; } = new Dictionary<string, string>()
        {
            { "MudBlazor", "https://github.com/Garderoben/MudBlazor" },
            { "AutoFixture", "https://github.com/AutoFixture/AutoFixture" },
            { "Bindables", "https://github.com/yusuf-gunaydin/Bindables" },
            { "Fluent Assertions", "https://fluentassertions.com" },
            { "Fody", "https://github.com/Fody/Fody" },
            { "Microsoft - ASP.NET", "https://asp.net" },
            { "NUnit", "https://nunit.org" },
            { "PropertyChanged", "https://github.com/Fody/PropertyChanged" },
            { "Serilog", "https://serilog.net" },
            { "TextCopy", "https://github.com/CopyText/TextCopy" },
            { "WindowsHook", "https://github.com/topstarai/WindowsHook" }
        };
        public AboutViewModel(IMediator mediator, ILogger logger)
        {
            this.mediator = mediator;
            this.logger = logger;

            VersionNumber = GetType().Assembly.GetName().Version.ToString();

            try
            {
                OperatingSystem = Environment.OSVersion.VersionString;
                EnvironmentVersion = Environment.Version.ToString();
            }
            catch (Exception e)
            {
                logger.LogInformation($"[About Page] - Failed to load Operating System details: " + e.Message);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task Hyperlink_RequestNavigate(Uri uri)
        {
            Console.WriteLine("debug");
            await mediator.Send(new OpenBrowserCommand(uri));

        }
    }
}
