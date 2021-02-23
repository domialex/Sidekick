using System;
using ElectronNET.API.Entities;

namespace Sidekick.Presentation.Blazor.Electron.App
{
    /// <summary>
    /// Service that holds the cookie name and value for <see cref="ElectronCookieProtectionMiddleware"/>.
    /// </summary>
    public class ElectronCookieProtection
    {
        public string Name { get; init; } = "ElectronCookieProtection";
        public string Value { get; init; } = Guid.NewGuid().ToString();
        public CookieDetails Cookie { get; init; }

        public ElectronCookieProtection()
        {
            Name = "ElectronCookieProtection";
            Value = Guid.NewGuid().ToString();
            Cookie = new CookieDetails()
            {
                Name = Name,
                Value = Value,
                Url = "http://localhost",
            };
        }
    }
}
