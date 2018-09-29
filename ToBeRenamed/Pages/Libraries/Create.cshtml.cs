using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToBeRenamed.Commands;
using ToBeRenamed.Dtos;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages.Libraries
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IMediator _mediator;

        [BindProperty]
        public LibraryDto Library { get; set; }

        public CreateModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userDto = _mediator.Send(new GetSignedInUserDto(User)).GetAwaiter().GetResult();

            await _mediator.Send(new CreateLibrary(userDto.Id, Library));
            
            return RedirectToPage("/Libraries/Index");
        }
    }
}