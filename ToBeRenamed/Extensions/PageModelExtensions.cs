using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ToBeRenamed.Extensions
{
    public static class PageModelExtensions
    {
        public static IActionResult InsufficientPrivileges(this PageModel pageModel)
        {
            return pageModel.RedirectToPage("/Index");
        }
    }
}
