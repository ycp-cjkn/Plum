using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyModel;
using ToBeRenamed.Commands;
using ToBeRenamed.Dtos;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages.Videos
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
        public string Title { get; set; }

        [BindProperty]
        public string Link { get; set; }

        [BindProperty]
        public string Description { get; set; }


        public async Task<IActionResult> OnPostAsync(int id)
        {
            var userDto = await _mediator.Send(new GetSignedInUserDto(User));

            var libraryDto = await _mediator.Send(new GetLibraryDtoById(id));

            await _mediator.Send(new CreateVideo(userDto.Id, libraryDto.Id ,Title, Link, Description));

            return Redirect($"/Library/{libraryDto.Id}");
        }
    }
}