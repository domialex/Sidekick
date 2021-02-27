using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Sidekick.Presentation.Blazor.Electron.App
{
    /// <summary>
    /// Middleware that checks if the requests being made are from the Electron app and nothing else.
    /// </summary>
    public class ElectronCookieProtectionMiddleware
    {
        private RequestDelegate Next { get; init; }
        private ElectronCookieProtection ElectronAuthorizationCookie { get; init; }

        public ElectronCookieProtectionMiddleware(RequestDelegate next, ElectronCookieProtection electronAuthorizationCookie)
        {
            Next = next;
            ElectronAuthorizationCookie = electronAuthorizationCookie;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Request.Cookies.TryGetValue(ElectronAuthorizationCookie.Name, out string cookie);

            if (cookie != ElectronAuthorizationCookie.Value)
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }

            await Next.Invoke(context);
        }

    }
}
