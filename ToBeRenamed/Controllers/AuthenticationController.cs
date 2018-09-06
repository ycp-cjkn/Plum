using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Extensions;
using ToBeRenamed.Models;

namespace ToBeRenamed.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        public AuthenticationController(IAuthenticationSchemeProvider schemeProvider)
        {
            _schemeProvider = schemeProvider;
        }

        [HttpGet]
        public async Task<IActionResult> SignIn(string returnUrl = "/")
        {
            var schemes = await GetExternalProvidersAsync().ConfigureAwait(false);

            return View("SignIn", new SignInViewModel
            {
                ReturnUrl = returnUrl,
                AuthenticationSchemes = schemes
            });
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromForm] SignInRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Provider))
            {
                return BadRequest();
            }

            if (await IsProviderSupportedAsync(request.Provider).ConfigureAwait(false))
            {
                return BadRequest();
            }

            var properties = new AuthenticationProperties { RedirectUri = request.ReturnUrl };

            return Challenge(properties, request.Provider);
        }

        [HttpGet]
        public IActionResult SignOut()
        {
            var properties = new AuthenticationProperties { RedirectUri = "/" };

            const string scheme = CookieAuthenticationDefaults.AuthenticationScheme;

            return SignOut(properties, scheme);
        }

        private async Task<IEnumerable<AuthenticationScheme>> GetExternalProvidersAsync()
        {
            var schemes = await _schemeProvider.GetAllSchemesAsync().ConfigureAwait(false);

            return schemes.Where(s => !string.IsNullOrEmpty(s.DisplayName));
        }

        private async Task<bool> IsProviderSupportedAsync(string provider)
        {
            var schemes = await GetExternalProvidersAsync().ConfigureAwait(false);

            return !schemes.Any(s => s.Name.EqualsIgnoreCase(provider));
        }
    }
}
