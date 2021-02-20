using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Sidekick.Application;
using Sidekick.Application.Settings;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Infrastructure;
using Sidekick.Localization;
using Sidekick.Logging;
using Sidekick.Mapper;
using Sidekick.Mediator;
using Sidekick.Persistence;
using Sidekick.Platform;
using Sidekick.Presentation.Blazor.Mock;

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
            services
                .AddRazorPages()
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.Load("Sidekick.Presentation.Blazor"));
                });
            services.AddServerSideBlazor();

            services
                // Common
                .AddSidekickMapper(
                    Assembly.Load("Sidekick.Infrastructure"),
                    Assembly.Load("Sidekick.Persistence"))
                .AddSidekickMediator(
                    Assembly.Load("Sidekick.Application"),
                    Assembly.Load("Sidekick.Domain"),
                    Assembly.Load("Sidekick.Infrastructure"),
                    Assembly.Load("Sidekick.Persistence"),
                    Assembly.Load("Sidekick.Presentation"),
                    Assembly.Load("Sidekick.Presentation.Blazor"),
                    Assembly.Load("Sidekick.Presentation.Blazor.Mock"))

                // Layers
                .AddSidekickApplication()
                .AddSidekickLogging()
                .AddSidekickInfrastructure()
                .AddSidekickLocalization()
                .AddSidekickPersistence()
                .AddSidekickPlatform()
                .AddSidekickPresentation()
                .AddSidekickPresentationBlazor()
                .AddSidekickPresentationBlazorMock();

            services
                .AddMudServices()
                .AddMudBlazorDialog()
                .AddMudBlazorSnackbar()
                .AddMudBlazorResizeListener()
                .AddMudBlazorDom();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, IMediator mediator, ISidekickSettings settings)
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
                    Language_Parser = !string.IsNullOrEmpty(settings.Language_Parser) ? settings.Language_Parser : "en",
                    Language_UI = !string.IsNullOrEmpty(settings.Language_UI) ? settings.Language_UI : "en",
                    LeagueId = !string.IsNullOrEmpty(settings.LeagueId) ? settings.LeagueId : "Ritual",
                }, true));
                await mediator.Send(new InitializeCommand(true));
            });
        }
    }
}
