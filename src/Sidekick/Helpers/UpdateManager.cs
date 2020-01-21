using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Diagnostics;

namespace Sidekick.Helpers
{
    /* Implement in Program.cs
    var updateManager = new UpdateManager();
    if (updateManager.CheckUpdate())
    {
        if(MessageBox.Show("There is a new update of Sidekick available. Download and install?", "AutoUpdater", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            if (updateManager.UpdateSidekick())
            {
                ProcessHelper.mutex = null;
                updateManager.Restart();
            }
            else
            {
                MessageBox.Show("Update failed!");                       
            }
            return;
        }              
    }
    */

    public class UpdateManager
    {
        #region Github API
        public class GithubRelease
        {
            public string Name { get; set; }
            public Asset[] Assets { get; set; }
        }

        public class Asset
        {
            public string Url { get; set; }
            public string Name { get; set; }
            [JsonPropertyName("browser_download_url")]
            public string DownloadUrl { get; set; }
        }
        #endregion

        private readonly string INSTALL_DIR;
        private readonly string TMP_DIR;
        private readonly string ZIP_PATH;
        
        public UpdateManager()
        {
            INSTALL_DIR = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            TMP_DIR = Path.Combine(INSTALL_DIR, "tmp");
            ZIP_PATH = Path.Combine(INSTALL_DIR, "update.zip");
        }

        /// <summary>
        /// Checks if there is a newer release available on github
        /// </summary>
        /// <returns></returns>
        public bool CheckUpdate()
        {
            // TODO
            return true;
        }

        /// <summary>
        /// Trys to update sidekick
        /// </summary>
        /// <returns></returns>
        public bool UpdateSidekick()
        {
            //TODO: ProgressBar
            if (Task.Run(() => DownloadNewestRelease()).Result)
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
        /// Extracts the files from the downloaded zip and deletes the zip file
        /// </summary>
        private void ApplyUpdate()
        {
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
            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://api.github.com");
                httpClient.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await httpClient.GetAsync("/repos/domialex/Sidekick/releases/latest");
                GithubRelease githubRelease = null;

                var jsonOptions = new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true };

                if (response.IsSuccessStatusCode)
                {
                    githubRelease = await JsonSerializer.DeserializeAsync<GithubRelease>(await response.Content.ReadAsStreamAsync(), jsonOptions);
                }
                else
                {
                    //Get List of releases if there is no correct latest release ( should only happen if there are only pre-releases)
                    var listResponse = await httpClient.GetAsync("/repos/domialex/Sidekick/releases");
                    if (listResponse.IsSuccessStatusCode)
                    {
                        var githubReleaseList = await JsonSerializer.DeserializeAsync<GithubRelease[]>(await listResponse.Content.ReadAsStreamAsync(), jsonOptions);
                        githubRelease = githubReleaseList[0];
                    }                   
                }

                if (githubRelease != null)
                {
                    //download zip file and save to disk
                    using (Stream contentStream = await (await httpClient.GetAsync(githubRelease.Assets[0].DownloadUrl)).Content.ReadAsStreamAsync(), stream = new FileStream(ZIP_PATH, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await contentStream.CopyToAsync(stream);
                    }
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
                if (Directory.Exists(TMP_DIR))
                {
                    Directory.Delete(TMP_DIR, true);
                    Directory.CreateDirectory(TMP_DIR);
                }
                else
                {
                    Directory.CreateDirectory(TMP_DIR);
                }

                foreach (var file in Directory.EnumerateFiles(INSTALL_DIR))
                {
                    if(!file.EndsWith(".zip"))
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
            Process.Start(Path.Combine(INSTALL_DIR, "Sidekick.exe"));
        }
    }
}
