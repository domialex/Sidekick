using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sidekick.Modules.Initialization
{
    public class InitializationMiddleware
    {
        private readonly RequestDelegate next;

        public InitializationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (!Pages.Initialization.HasRun)
            {
                httpContext.Response.Redirect("/initialize");
            }

            await next(httpContext);
        }
    }
}
