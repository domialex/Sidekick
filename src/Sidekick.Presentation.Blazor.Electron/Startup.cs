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
using MudBlazor.Services;
using Sidekick.Application;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Views;
using Sidekick.Infrastructure;
using Sidekick.Logging;
using Sidekick.Mapper;
using Sidekick.Mediator;
using Sidekick.Persistence;
using Sidekick.Platform;
using Sidekick.Presentation.Blazor.Electron.Tray;
using Sidekick.Presentation.Blazor.Electron.Views;

namespace Sidekick.Presentation.Blazor.Electron
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

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
                // Common
                .AddSidekickLogging()
                .AddSidekickMapper(
                    Assembly.Load("Sidekick.Infrastructure"),
                    Assembly.Load("Sidekick.Persistence"))
                .AddSidekickMediator(
                    Assembly.Load("Sidekick.Application"),
                    Assembly.Load("Sidekick.Domain"),
                    Assembly.Load("Sidekick.Infrastructure"),
                    Assembly.Load("Sidekick.Persistence"),
                    Assembly.Load("Sidekick.Platform"),
                    Assembly.Load("Sidekick.Presentation"),
                    Assembly.Load("Sidekick.Presentation.Blazor"),
                    Assembly.Load("Sidekick.Presentation.Blazor.Electron"))

                // Layers
                .AddSidekickApplication()
                .AddSidekickInfrastructure()
                .AddSidekickPersistence()
                .AddSidekickPlatform()
                .AddSidekickPresentation()
                .AddSidekickPresentationBlazor();

            services.AddSingleton<TrayProvider>();
            services.AddSingleton<IViewLocator, ViewLocator>();

            services
                .AddMudServices()
                .AddMudBlazorDialog()
                .AddMudBlazorSnackbar()
                .AddMudBlazorResizeListener()
                .AddMudBlazorDom();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider,
            TrayProvider trayProvider,
            IMediator mediator)
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

            // Electron stuff
            ElectronNET.API.Electron.WindowManager.IsQuitOnWindowAllClosed = false;
            Task.Run(async () =>
            {
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
                await Task.Delay(50);
                browserWindow.Close();

                // Initialize Sidekick
                await mediator.Send(new InitializeCommand(true));
            });
        }
    }
}
