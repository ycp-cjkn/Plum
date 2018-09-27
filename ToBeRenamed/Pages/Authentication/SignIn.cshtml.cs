using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Extensions;

namespace ToBeRenamed.Pages.Authentication
{
    public class SignInModel : PageModel
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        public SignInModel(IAuthenticationSchemeProvider schemeProvider)
        {
            _schemeProvider = schemeProvider;
        }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty]
        public string Provider { get; set; }

        public IEnumerable<AuthenticationScheme> AuthenticationSchemes { get; set; }

        public async Task OnGetAsync()
        {
            AuthenticationSchemes = await GetExternalProvidersAsync().ConfigureAwait(false);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrWhiteSpace(Provider))
            {
                return BadRequest();
            }

            if (await IsProviderSupportedAsync(Provider).ConfigureAwait(false))
            {
                return BadRequest();
            }

            var properties = new AuthenticationProperties { RedirectUri = ReturnUrl ?? "/" };

            return Challenge(properties, Provider);
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