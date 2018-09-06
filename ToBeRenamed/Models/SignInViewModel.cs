using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;

namespace ToBeRenamed.Models
{
    public class SignInViewModel
    {
        public string ReturnUrl { get; set; }
        public IEnumerable<AuthenticationScheme> AuthenticationSchemes { get; set; }
    }
}
