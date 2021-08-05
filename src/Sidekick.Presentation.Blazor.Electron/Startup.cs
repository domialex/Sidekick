using System;
using System.Reflection;
using System.Threading.Tasks;
using ElectronNET.API.Entities;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sidekick.Apis.GitHub;
using Sidekick.Application;
using Sidekick.Common.Platform;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Views;
using Sidekick.Infrastructure;
using Sidekick.Localization;
using Sidekick.Logging;
using Sidekick.Mapper;
using Sidekick.Mediator;
using Sidekick.Modules.Cheatsheets;
using Sidekick.Modules.Settings;
using Sidekick.Persistence;
using Sidekick.Presentation.Blazor.Electron.Keybinds;
using Sidekick.Presentation.Blazor.Electron.Tray;
using Sidekick.Presentation.Blazor.Electron.Views;

namespace Sidekick.Presentation.Blazor.Electron
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment environment;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRazorPages()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.Load("Sidekick.Presentation.Blazor"));
                });
            services.AddServerSideBlazor();

            services
                // Layers
                .AddSidekickApplication(configuration)
                .AddSidekickInfrastructure()
                .AddSidekickLocalization()
                .AddSidekickPersistence()
                .AddSidekickPlatform()
                .AddSidekickPresentationBlazor()

                // Common
                .AddSidekickLogging(configuration, environment)
                .AddSidekickMapper()
                .AddSidekickMediator()

                // Apis
                .AddSidekickGitHubApi()

                // Modules
                .AddSidekickCheatsheets()
                .AddSidekickSettings(configuration);

            services.AddSingleton<TrayProvider>();
            services.AddSingleton<ViewLocator>();
            services.AddSingleton<IViewLocator>(implementationFactory: (sp) => sp.GetRequiredService<ViewLocator>());
            services.AddScoped<IViewInstance, ViewInstance>();
            services.AddSingleton<IKeybindProvider, KeybindProvider>();
            services.AddSingleton<ElectronCookieProtection>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider,
            TrayProvider trayProvider,
            IMediator mediator,
            ILogger<Startup> logger)
        {
            serviceProvider.UseSidekickMapper();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<ElectronCookieProtectionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            // Electron stuff
            ElectronNET.API.Electron.NativeTheme.SetThemeSource(ThemeSourceMode.Dark);
            ElectronNET.API.Electron.WindowManager.IsQuitOnWindowAllClosed = false;
            Task.Run(async () =>
            {
                try
                {
                    // Tray
                    trayProvider.Initialize();

                    // We need to trick Electron into thinking that our app is ready to be opened.
                    // This makes Electron hide the splashscreen. For us, it means we are ready to initialize and price check :)
                    var browserWindow = await ElectronNET.API.Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
                    {
                        Width = 1,
                        Height = 1,
                        Frame = false,
                        Show = true,
                        Transparent = true,
                        Fullscreenable = false,
                        Minimizable = false,
                        Maximizable = false,
                        SkipTaskbar = true,
                        WebPreferences = new WebPreferences()
                        {
                            NodeIntegration = false,
                        }
                    });
                    browserWindow.WebContents.OnCrashed += (killed) => ElectronNET.API.Electron.App.Exit();
                    await Task.Delay(50);
                    browserWindow.Close();

                    // Initialize Sidekick
                    await mediator.Send(new InitializeCommand(true, true));
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Could not initialize Sidekick.");
                    ElectronNET.API.Electron.App.Exit();
                }
            });
        }
    }
}
