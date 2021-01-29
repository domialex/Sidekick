using System;
using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Sidekick.Application;
using Sidekick.Application.Settings;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Domain.Views;
using Sidekick.Infrastructure;
using Sidekick.Logging;
using Sidekick.Mapper;
using Sidekick.Mediator;
using Sidekick.Persistence;
using Sidekick.Platform;
using Sidekick.Presentation.Blazor.Mocks;

namespace Sidekick.Presentation.Blazor
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
            services.AddRazorPages();
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
                    Assembly.Load("Sidekick.Presentation.Blazor"))

                // Layers
                .AddSidekickApplication()
                .AddSidekickInfrastructure()
                .AddSidekickPersistence()
                .AddSidekickPlatform()
                .AddSidekickPresentation();

            services
                .AddMudBlazorDialog()
                .AddMudBlazorSnackbar()
                .AddMudBlazorResizeListener();

            services.AddSingleton<IViewLocator, ViewLocator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, IMediator mediator)
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

            Task.Run(async () =>
            {
                await mediator.Send(new SaveSettingsCommand(new SidekickSettings()
                {
                    LeagueId = "Ritual",
                    Language_Parser = "en",
                    Language_UI = "en",
                }));

                await mediator.Send(new InitializeCommand(true));
            });

        }
    }
}
