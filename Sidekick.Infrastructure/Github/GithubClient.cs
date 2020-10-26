using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Sidekick.Infrastructure.Github
{
    public class GithubClient : IGithubClient
    {
        public GithubClient(IHttpClientFactory httpClientFactory)
        {
            Client = httpClientFactory.CreateClient();
            Client.BaseAddress = new Uri("https://api.github.com");
            Client.DefaultRequestHeaders.UserAgent.TryParseAdd("request");
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public HttpClient Client { get; private set; }
    }
}
