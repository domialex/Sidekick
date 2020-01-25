using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Sidekick.Core.Update.Github_API;
using System.Linq;


namespace Sidekick.Core.Update
{
    public class UpdateManager : IUpdateManager
    {
        private readonly string INSTALL_DIR;
        private readonly string TMP_DIR;
        private readonly string ZIP_PATH;

        private GithubRelease _latestRelease;

        private HttpClient _httpClient;

        private Action<string, int> reportProgress;
        public Action<string, int> ReportProgress { set { reportProgress = value; } }

        public UpdateManager(IHttpClientFactory httpClientFactory)
        {           
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://api.github.com");
            _httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            INSTALL_DIR = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            TMP_DIR = Path.Combine(INSTALL_DIR, "UpdateBackup");
            ZIP_PATH = Path.Combine(INSTALL_DIR, "update.zip");
        }

        /// <summary>
        /// Checks if there is a newer release available on github
        /// </summary>
        /// <returns></returns>
        public async Task<bool> NewVersionAvailable()
        {
            reportProgress.Invoke("Checking for Updates...", 0);
            _latestRelease = await GetLatestRelease();
            if (_latestRelease != null)
            {
                var latestVersion = new Version(Regex.Match(_latestRelease.Tag, @"(\d+\.){2}\d+").ToString());
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
            reportProgress.Invoke("Applying update...", 80);
            ZipFile.ExtractToDirectory(ZIP_PATH, INSTALL_DIR);
            File.Delete(ZIP_PATH);
        }
        /// <summary>
        /// Restores the backuped files
        /// </summary>
        private void RollbackUpdate()
        {
            try
            {
                foreach (var file in Directory.EnumerateFiles(TMP_DIR))
                {
                    var fileName = Path.GetFileName(file);
                    File.Move(file, Path.Combine(INSTALL_DIR, fileName));
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
            reportProgress.Invoke("Downloading latest release from GitHub...", 30);
            if (_latestRelease != null)
            {
                if (File.Exists(ZIP_PATH)) File.Delete(ZIP_PATH);
                //download zip file and save to disk
                using (Stream contentStream = await (await _httpClient.GetAsync(_latestRelease.Assets[0].DownloadUrl)).Content.ReadAsStreamAsync(), stream = new FileStream(ZIP_PATH, FileMode.Create, FileAccess.Write, FileShare.None))
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
                reportProgress.Invoke("Backuping current version...", 50);
                if (Directory.Exists(TMP_DIR))
                {
                    Directory.Delete(TMP_DIR, true);
                    Directory.CreateDirectory(TMP_DIR);
                }
                else
                {
                    Directory.CreateDirectory(TMP_DIR);
                }

                // Backup resource folders etc.
                foreach (var directory in Directory.EnumerateDirectories(INSTALL_DIR))
                {
                    if (directory != TMP_DIR)
                        Directory.Move(directory, Path.Combine(TMP_DIR, new DirectoryInfo(directory).Name));
                }

                // Backup install files
                foreach (var file in Directory.EnumerateFiles(INSTALL_DIR))
                {
                    //keep settings and already downloaded file 
                    if(!file.EndsWith(".zip") && !file.EndsWith(".json"))
                    {
                        var fileName = Path.GetFileName(file);
                        File.Move(file, Path.Combine(TMP_DIR, fileName));
                    }                    
                }
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Restarts the application
        /// </summary>
        public void Restart()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = Path.Combine(INSTALL_DIR, "Sidekick.exe"),
                UseShellExecute = false,
            };
            Process.Start(startInfo);
        }
    }
}
