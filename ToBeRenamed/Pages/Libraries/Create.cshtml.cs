using System.ComponentModel.DataAnnotations;
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
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;

        public CreateModel(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [BindProperty]
        [Required]
        public string Title { get; set; }
        
        [BindProperty]
        [Required]
        public string Description { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userDto = await _mediator.Send(new GetSignedInUserDto(User));

            await _mediator.Send(new CreateLibrary(userDto.Id, Title, Description));

            return RedirectToPage("/Libraries/Index");
        }
    }
}