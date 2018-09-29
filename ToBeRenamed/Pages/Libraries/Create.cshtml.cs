using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToBeRenamed.Dtos;

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

//            public IEnumerable<LibraryDto> Libraries { get; set; }

//            public void OnGet()
//            {
//                var userDto = _mediator.Send(new GetSignedInUserDto(User)).GetAwaiter().GetResult();
//
//                Libraries = _mediator.Send(new GetLibrariesCreatedByUserId(userDto.Id)).GetAwaiter().GetResult();
//            }
    }
}