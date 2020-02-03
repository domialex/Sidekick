using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;
using Sidekick.Core.Update.Github_API;


namespace Sidekick.Core.Update
{
    public class UpdateManager : IUpdateManager, IOnBeforeInit
    {
        private readonly HttpClient _httpClient;
        private readonly IInitializer initializer;

        public UpdateManager(IHttpClientFactory httpClientFactory,
            IInitializer initializer)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://api.github.com");
            _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            this.initializer = initializer;
        }

        public string InstallDirectory => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private string TempDirectory => Path.Combine(InstallDirectory, "UpdateBackup");

        private string ZipPath => Path.Combine(InstallDirectory, "update.zip");

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
                if (BackupFiles())
                {
                    ApplyUpdate();
                    return true;
                }
            }

            RollbackUpdate();
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
        /// Extracts the files from the downloaded zip and deletes the zip file
        /// </summary>
        private void ApplyUpdate()
        {
            initializer.ReportProgress(ProgressTypeEnum.Other, nameof(UpdateManager), "Applying update...");
            ZipFile.ExtractToDirectory(ZipPath, InstallDirectory);
            File.Delete(ZipPath);
        }
        /// <summary>
        /// Restores the backuped files
        /// </summary>
        private void RollbackUpdate()
        {
            try
            {
                foreach (var file in Directory.EnumerateFiles(TempDirectory))
                {
                    var fileName = Path.GetFileName(file);
                    File.Move(file, Path.Combine(InstallDirectory, fileName));
                }
            }
            catch { }
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
                if (File.Exists(ZipPath)) File.Delete(ZipPath);
                //download zip file and save to disk
                using (Stream contentStream = await (await _httpClient.GetAsync(LatestRelease.Assets[0].DownloadUrl)).Content.ReadAsStreamAsync(), stream = new FileStream(ZipPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await contentStream.CopyToAsync(stream);
                }
            }

            return true;
        }
        /// <summary>
        /// Backups the files of the current installation
        /// </summary>
        /// <returns></returns>
        private bool BackupFiles()
        {
            try
            {
                initializer.ReportProgress(ProgressTypeEnum.Other, nameof(UpdateManager), "Backuping current version...");
                if (Directory.Exists(TempDirectory))
                {
                    Directory.Delete(TempDirectory, true);
                    Directory.CreateDirectory(TempDirectory);
                }
                else
                {
                    Directory.CreateDirectory(TempDirectory);
                }

                // Backup resource folders etc.
                foreach (var directory in Directory.EnumerateDirectories(InstallDirectory))
                {
                    if (directory != TempDirectory)
                    {
                        Directory.Move(directory, Path.Combine(TempDirectory, new DirectoryInfo(directory).Name));
                    }
                }

                // Backup install files
                foreach (var file in Directory.EnumerateFiles(InstallDirectory))
                {
                    //keep settings and already downloaded file 
                    if (!file.EndsWith(".zip") && !file.EndsWith(".json"))
                    {
                        var fileName = Path.GetFileName(file);
                        File.Move(file, Path.Combine(TempDirectory, fileName));
                    }
                }
                return true;
            }
            catch { return false; }
        }
    }
}
