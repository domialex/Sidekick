using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sidekick.Common.Blazor
{
    public class StartupMiddleware
    {
        private static bool HasStartup { get; set; } = false;
        private static readonly Regex IgnorePaths = new Regex("^\\/_blazor\\/.*$");
        internal static string StartupPath { get; set; }

        private readonly RequestDelegate next;

        public StartupMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task InvokeAsync(HttpContext httpContext)
        {
            if (!HasStartup && !IgnorePaths.IsMatch(httpContext.Request.Path.Value))
            {
                HasStartup = true;

                if (httpContext.Request.Path.Value != StartupPath)
                {
                    httpContext.Response.Redirect(StartupPath);
                    return httpContext.Response.CompleteAsync();
                }
            }

            return next(httpContext);
        }
    }
}
