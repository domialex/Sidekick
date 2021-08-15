using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Apis.GitHub.Localization;
using Sidekick.Apis.GitHub.Models;
using Sidekick.Common;
using Sidekick.Common.Initialization;

namespace Sidekick.Apis.GitHub
{
    public class GitHubClient : IGitHubClient
    {
        private readonly ILogger<GitHubClient> logger;
        private readonly UpdateResources resources;
        private readonly IInitializationProvider initializationProvider;
        private readonly HttpClient client;

        public GitHubClient(
            IHttpClientFactory httpClientFactory,
            ILogger<GitHubClient> logger,
            UpdateResources resources,
            IInitializationProvider initializationProvider)
        {
            this.logger = logger;
            this.resources = resources;
            this.initializationProvider = initializationProvider;
            client = httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://api.github.com");
            client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> Update()
        {
            try
            {
                var release = await GetLatestRelease();
                if (IsUpdateAvailable(release))
                {
                    await initializationProvider.OnProgress(0, resources.Downloading(release.Tag));

                    var path = await DownloadRelease(release);
                    if (path == null)
                    {
                        await initializationProvider.OnProgress(33, resources.Failed);
                        await Task.Delay(1000);
                        await initializationProvider.OnProgress(66, resources.Failed);
                        await Task.Delay(1000);
                        await initializationProvider.OnProgress(100, resources.Failed);
                        await Task.Delay(1000);
                        return false;
                    }

                    await initializationProvider.OnProgress(100, resources.Downloading(release.Tag));
                    await Task.Delay(1000);

                    Process.Start(path);

                    return true;
                }
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "Update failed.");
            }

            return false;
        }

        /// <summary>
        /// Determines latest release on github. Pre-releases do not count as release, therefore we need to get the list of releases first, if no actual latest release can be found
        /// </summary>
        /// <returns></returns>
        private async Task<GitHubRelease> GetLatestRelease()
        {
            // Get List of releases
            var listResponse = await client.GetAsync("/repos/domialex/Sidekick/releases");
            if (listResponse.IsSuccessStatusCode)
            {
                var githubReleaseList = await JsonSerializer.DeserializeAsync<GitHubRelease[]>(await listResponse.Content.ReadAsStreamAsync(), new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                return githubReleaseList.FirstOrDefault(x => !x.Prerelease);
            }

            return null;
        }

        /// <summary>
        /// Determines if there is a newer version available
        /// </summary>
        /// <returns></returns>
        private bool IsUpdateAvailable(GitHubRelease release)
        {
            if (release != null)
            {
                logger.LogInformation("[Updater] Found " + release.Tag + " as latest version on GitHub.");

                var latestVersion = new Version(Regex.Match(release.Tag, @"(\d+\.){2}\d+").ToString());
                var currentVersion = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.Contains("Sidekick")).GetName().Version;

                var result = currentVersion.CompareTo(latestVersion);
                return result < 0;
            }
            else
            {
                logger.LogInformation("[Updater] No latest release found on GitHub.");
            }

            return false;
        }

        /// <summary>
        /// Downloads the latest release from github
        /// </summary>
        /// <returns></returns>
        private async Task<string> DownloadRelease(GitHubRelease release)
        {
            if (release == null) return null;

            var downloadPath = SidekickPaths.GetDataFilePath("Sidekick-Update.exe");
            if (File.Exists(downloadPath)) File.Delete(downloadPath);

            var downloadUrl = release.Assets.FirstOrDefault(x => x.Name == "Sidekick-Setup.exe")?.DownloadUrl;
            if (downloadUrl == null) return null;

            var response = await client.GetAsync(downloadUrl);
            using var downloadStream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await downloadStream.CopyToAsync(fileStream);

            return downloadPath;
        }
    }
}
