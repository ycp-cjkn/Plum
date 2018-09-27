using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using MediatR;
using ToBeRenamed.Dtos;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages
{
    [Authorize]
    public class LibrariesModel : PageModel
    {
        private readonly IMediator _mediator;

        public LibrariesModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IEnumerable<LibraryDto> Libraries { get; set; }

        public void OnGet()
        {
            var userDto = _mediator.Send(new GetSignedInUserDto(User)).GetAwaiter().GetResult();

            Libraries = _mediator.Send(new GetLibrariesCreatedByUserId(userDto.Id)).GetAwaiter().GetResult();
        }
    }
}