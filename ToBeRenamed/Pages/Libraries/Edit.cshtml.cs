using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages.Libraries
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;

        public EditModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty]
        [Required]
        public string NewTitle { get; set; }

        [BindProperty]
        [Required]
        public string NewDescription { get; set; }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _mediator.Send(new UpdateLibraryInfo(id, NewTitle, NewDescription));
            return RedirectToPage("/Libraries/Index");
        }
    }
}