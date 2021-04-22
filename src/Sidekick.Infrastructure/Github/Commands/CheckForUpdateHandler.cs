using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Apis.GitHub;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Extensions;
using Sidekick.Infrastructure.Github.Models;
using Sidekick.Localization.Update;

namespace Sidekick.Infrastructure.Github.Queries
{
    public class CheckForUpdateHandler : IQueryHandler<CheckForUpdate, bool>
    {
        private readonly IMediator mediator;
        private readonly ILogger<CheckForUpdateHandler> logger;
        private readonly IGithubClient githubClient;
        private readonly UpdateResources resources;

        public CheckForUpdateHandler(
            IMediator mediator,
            ILogger<CheckForUpdateHandler> logger,
            IGithubClient githubClient,
            UpdateResources resources)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.githubClient = githubClient;
            this.resources = resources;
        }

        public async Task<bool> Handle(CheckForUpdate request, CancellationToken cancellationToken)
        {
            try
            {
                var release = await GetLatestRelease();
                if (IsUpdateAvailable(release))
                {
                    await mediator.Publish(new InitializationProgressed(0)
                    {
                        Title = resources.Downloading(release.Tag),
                    });

                    var path = await DownloadRelease(release);
                    if (path == null)
                    {
                        await mediator.Publish(new InitializationProgressed(33)
                        {
                            Title = resources.Failed,
                        });
                        await Task.Delay(1000);
                        await mediator.Publish(new InitializationProgressed(66)
                        {
                            Title = resources.Failed,
                        });
                        await Task.Delay(1000);
                        await mediator.Publish(new InitializationProgressed(100)
                        {
                            Title = resources.Failed,
                        });
                        await Task.Delay(1000);
                        return false;
                    }

                    await mediator.Publish(new InitializationProgressed(100)
                    {
                        Title = resources.Downloading(release.Tag),
                    });
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
        private async Task<GithubRelease> GetLatestRelease()
        {
            // Get List of releases
            var listResponse = await githubClient.Client.GetAsync("/repos/domialex/Sidekick/releases");
            if (listResponse.IsSuccessStatusCode)
            {
                var githubReleaseList = await JsonSerializer.DeserializeAsync<GithubRelease[]>(await listResponse.Content.ReadAsStreamAsync(), new JsonSerializerOptions { IgnoreNullValues = true, PropertyNameCaseInsensitive = true });
                return githubReleaseList.FirstOrDefault(x => !x.Prerelease);
            }

            return null;
        }

        /// <summary>
        /// Determines if there is a newer version available
        /// </summary>
        /// <returns></returns>
        private bool IsUpdateAvailable(GithubRelease release)
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
        private async Task<string> DownloadRelease(GithubRelease release)
        {
            if (release == null) return null;

            var downloadPath = SidekickPaths.GetDataFilePath("Sidekick-Update.exe");
            if (File.Exists(downloadPath)) File.Delete(downloadPath);

            var downloadUrl = release.Assets.FirstOrDefault(x => x.Name == "Sidekick-Setup.exe")?.DownloadUrl;
            if (downloadUrl == null) return null;

            var response = await githubClient.Client.GetAsync(downloadUrl);
            using var downloadStream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(downloadPath, FileMode.Create, FileAccess.Write, FileShare.None);
            await downloadStream.CopyToAsync(fileStream);

            return downloadPath;
        }
    }
}
