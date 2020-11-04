using System;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Initialization.Queries;
using Sidekick.Infrastructure.Github.Models;

namespace Sidekick.Infrastructure.Github.Queries
{
    public class IsNewVersionAvailableHandler : IQueryHandler<IsNewVersionAvailableQuery, bool>
    {
        private readonly IGithubClient githubClient;

        public IsNewVersionAvailableHandler(
            IGithubClient githubClient)
        {
            this.githubClient = githubClient;
        }

        public async Task<bool> Handle(IsNewVersionAvailableQuery request, CancellationToken cancellationToken)
        {
            var latestRelease = await GetLatestRelease();
            if (latestRelease != null)
            {
                var latestVersion = new Version(Regex.Match(latestRelease.Tag, @"(\d+\.){2}\d+").ToString());
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
            var response = await githubClient.Client.GetAsync("/repos/domialex/Sidekick/releases/latest");
            if (response.IsSuccessStatusCode)
            {
                latestRelease = await JsonSerializer.DeserializeAsync<GithubRelease>(await response.Content.ReadAsStreamAsync(), jsonOptions);
            }
            else
            {
                //Get List of releases if there is no latest release ( should only happen if there are only pre-releases)
                var listResponse = await githubClient.Client.GetAsync("/repos/domialex/Sidekick/releases");
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
    }
}
