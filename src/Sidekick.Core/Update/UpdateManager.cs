using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;
using Sidekick.Core.Natives;
using Sidekick.Core.Update.Github_API;


namespace Sidekick.Core.Update
{
    public class UpdateManager : IUpdateManager, IOnBeforeInit
    {
        private readonly HttpClient _httpClient;
        private readonly IInitializer initializer;
        private readonly INativeProcess nativeProcess;

        public UpdateManager(IHttpClientFactory httpClientFactory,
            IInitializer initializer,
            INativeProcess nativeProcess)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://api.github.com");
            _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.initializer = initializer;
            this.nativeProcess = nativeProcess;
        }

        public string InstallDirectory => Path.GetDirectoryName(AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.Contains("Sidekick")).Location);

        public string UpdateFullPath => Path.Combine(InstallDirectory, "Sidekick_Update.exe");

        private GithubRelease LatestRelease { get; set; }

        public Task OnBeforeInit()
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Checks if there is a newer release available on github
        /// </summary>
        /// <returns></returns>
        public async Task<bool> NewVersionAvailable()
        {
            initializer.ReportProgress(ProgressTypeEnum.Other, nameof(UpdateManager), "Checking for Updates...");
            LatestRelease = await GetLatestRelease();
            if (LatestRelease != null)
            {
                var latestVersion = new Version(Regex.Match(LatestRelease.Tag, @"(\d+\.){2}\d+").ToString());
                // var currentVersion = new Version("0.2.0");
                var currentVersion = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.Contains("Sidekick")).GetName().Version;

                var result = currentVersion.CompareTo(latestVersion);
                return result < 0 ? true : false;
            }

            return false;
        }

        /// <summary>
        /// Trys to update sidekick
        /// </summary>
        /// <returns></returns>
        public async Task<bool> UpdateSidekick()
        {
            if (await DownloadNewestRelease())
            {
                initializer.ReportProgress(ProgressTypeEnum.Other, nameof(UpdateManager), "Applying update...");
                return true;
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
            try
            {
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
            }
            catch (Exception)
            {
                throw;
            }
            return latestRelease;
        }

        /// <summary>
        /// Downloads the latest release from github
        /// </summary>
        /// <returns></returns>
        private async Task<bool> DownloadNewestRelease()
        {
            initializer.ReportProgress(ProgressTypeEnum.Other, nameof(UpdateManager), "Downloading latest release from GitHub...");
            if (LatestRelease != null)
            {
                var downloadUrl = LatestRelease.Assets
                    .ToList()
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
        }
    }
}
