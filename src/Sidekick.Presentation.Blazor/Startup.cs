using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sidekick.Apis.GitHub;
using Sidekick.Apis.Poe;
using Sidekick.Apis.PoeNinja;
using Sidekick.Apis.PoePriceInfo;
using Sidekick.Common;
using Sidekick.Common.Game;
using Sidekick.Common.Platform;
using Sidekick.Common.Settings;
using Sidekick.Mock;
using Sidekick.Modules.About;
using Sidekick.Modules.Cheatsheets;
using Sidekick.Modules.Development;
using Sidekick.Modules.Maps;
using Sidekick.Modules.Settings;
using Sidekick.Modules.Trade;

namespace Sidekick.Presentation.Blazor
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var mvcBuilder = services
                .AddRazorPages(options =>
                {
                    options.RootDirectory = "/Shared";
                })
                .AddFluentValidation(options =>
                {
                    options.RegisterValidatorsFromAssembly(Assembly.Load("Sidekick.Presentation.Blazor"));
                });
            services.AddServerSideBlazor();

            services.AddHttpClient();

            services
                // Layers
                .AddSidekickPresentationBlazor()

                // Common
                .AddSidekickCommon()
                .AddSidekickCommonGame()
                .AddSidekickCommonPlatform()

                // Apis
                .AddSidekickGitHubApi()
                .AddSidekickPoeApi()
                .AddSidekickPoeNinjaApi()
                .AddSidekickPoePriceInfoApi()

                // Modules
                .AddSidekickAbout()
                .AddSidekickCheatsheets()
                .AddSidekickDevelopment()
                .AddSidekickMaps()
                .AddSidekickSettings(configuration)
                .AddSidekickTrade()

                // Mocks
                .AddSidekickMocks();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, ISettingsService settingsService)
        {
            Task.Run(async () =>
            {
                await settingsService.Save(nameof(ISettings.Language_Parser), "en");
                await settingsService.Save(nameof(ISettings.Language_UI), "en");
                await settingsService.Save(nameof(ISettings.LeagueId), "Expedition");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<InitializationMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });

        }
    }
}
