using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Localization;
using Sidekick.Core.Natives;
using Sidekick.Core.Update.Github_API;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Natives.Initialization.Commands;

namespace Sidekick.Core.Update
{
    public class UpdateInitializationStartedHandler : INotificationHandler<UpdateInitializationStarted>
    {
        private readonly HttpClient _httpClient;
        private readonly INativeNotifications nativeNotifications;
        private readonly INativeBrowser nativeBrowser;
        private readonly IStringLocalizer<UpdateInitializationStartedHandler> localizer;
        private readonly IMediator mediator;

        public UpdateInitializationStartedHandler(
            IHttpClientFactory httpClientFactory,
            INativeNotifications nativeNotifications,
            INativeBrowser nativeBrowser,
            IStringLocalizer<UpdateInitializationStartedHandler> localizer,
            IMediator mediator)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://api.github.com");
            _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.nativeNotifications = nativeNotifications;
            this.nativeBrowser = nativeBrowser;
            this.localizer = localizer;
            this.mediator = mediator;
        }

        public string InstallDirectory => Path.GetDirectoryName(AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.Contains("Sidekick")).Location);

        public string UpdateFullPath => Path.Combine(InstallDirectory, "Sidekick_Update.exe");

        private GithubRelease LatestRelease { get; set; }

        public async Task Handle(UpdateInitializationStarted notification, CancellationToken cancellationToken)
        {
            if (await NewVersionAvailable())
            {
                nativeNotifications.ShowYesNo(
                    localizer["Available"],
                    localizer["Title"],
                    onYes: async () =>
                    {
                        nativeBrowser.Open(new Uri("https://github.com/domialex/Sidekick/releases"));
                        await mediator.Send(new ShutdownCommand());

                        //try
                        //{
                        //    if (await updateManagerService.UpdateSidekick())
                        //    {
                        //        nativeProcess.Mutex = null;
                        //        AdonisUI.Controls.MessageBox.Show(UpdateResources.UpdateCompleted, UpdateResources.Title, AdonisUI.Controls.MessageBoxButton.OK);

                        //        var startInfo = new ProcessStartInfo
                        //        {
                        //            FileName = Path.Combine(updateManagerService.InstallDirectory, "Sidekick.exe"),
                        //            UseShellExecute = false,
                        //        };
                        //        Process.Start(startInfo);
                        //    }
                        //    else
                        //    {
                        //        AdonisUI.Controls.MessageBox.Show(UpdateResources.UpdateFailed, UpdateResources.Title);
                        //        nativeBrowser.Open(new Uri("https://github.com/domialex/Sidekick/releases"));
                        //    }

                        //    Current.Shutdown();
                        //}
                        //catch (Exception)
                        //{
                        //    MessageBox.Show(UpdateResources.UpdateFailed, UpdateResources.Title);
                        //    nativeBrowser.Open(new Uri("https://github.com/domialex/Sidekick/releases"));
                        //}
                    });
            }
        }

        /// <summary>
        /// Checks if there is a newer release available on github
        /// </summary>
        /// <returns></returns>
        private async Task<bool> NewVersionAvailable()
        {
            LatestRelease = await GetLatestRelease();
            if (LatestRelease != null)
            {
                var latestVersion = new Version(Regex.Match(LatestRelease.Tag, @"(\d+\.){2}\d+").ToString());
                var currentVersion = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.Contains("Sidekick")).GetName().Version;

                var result = currentVersion.CompareTo(latestVersion);
                return result < 0;
            }

            return false;
        }

        /// <summary>
        /// Determines latest release on github. Pre-releases do not count as release, therefore we need to get the list of releases first, if no actual latest release can be found
        /// </summary>
        /// <returns></returns>
        private async Task<GithubRelease> GetLatestRelease()
        {
            GithubRelease latestRelease = null;

            var jsonOptions = new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true };
            var response = await _httpClient.GetAsync("/repos/domialex/Sidekick/releases/latest");
            if (response.IsSuccessStatusCode)
            {
                latestRelease = await JsonSerializer.DeserializeAsync<GithubRelease>(await response.Content.ReadAsStreamAsync(), jsonOptions);
            }
            else
            {
                //Get List of releases if there is no latest release ( should only happen if there are only pre-releases)
                var listResponse = await _httpClient.GetAsync("/repos/domialex/Sidekick/releases");
                if (listResponse.IsSuccessStatusCode)
                {
                    var githubReleaseList = await JsonSerializer.DeserializeAsync<GithubRelease[]>(await listResponse.Content.ReadAsStreamAsync(), jsonOptions);

                    // Iterate through version list to determine latest version
                    var highestVersionNo = new Version("0.0.0.0");
                    foreach (var release in githubReleaseList)
                    {
                        var releaseVersion = new Version(Regex.Match(release.Tag, @"(\d+\.){2}\d+").ToString());
                        if (highestVersionNo?.CompareTo(releaseVersion) < 0)
                        {
                            highestVersionNo = releaseVersion;
                            latestRelease = release;
                        }
                    }
                }
            }

            return latestRelease;
        }

        /// <summary>
        /// Trys to update sidekick
        /// </summary>
        /// <returns></returns>
        /*private async Task<bool> UpdateSidekick()
        {
            if (await DownloadNewestRelease())
            {
                return true;
            }

            return false;
        }*/

        /// <summary>
        /// Downloads the latest release from github
        /// </summary>
        /// <returns></returns>
        /*private async Task<bool> DownloadNewestRelease()
        {
            if (LatestRelease != null)
            {
                var downloadUrl = LatestRelease.Assets
                    .FirstOrDefault(x => x.DownloadUrl.EndsWith(".exe"))?
                    .DownloadUrl;

                if (File.Exists(UpdateFullPath) || string.IsNullOrEmpty(downloadUrl))
                {
                    return false;
                }

                var response = await _httpClient.GetAsync(downloadUrl);
                var responseStream = await response.Content.ReadAsStreamAsync();
                using var stream = new FileStream(UpdateFullPath, FileMode.Create, FileAccess.Write, FileShare.None);
                await responseStream.CopyToAsync(stream);
            }

            return true;
        }*/
    }
}
