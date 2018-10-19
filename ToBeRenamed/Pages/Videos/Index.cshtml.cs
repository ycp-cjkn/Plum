using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using MediatR;
using ToBeRenamed.Dtos;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages.Videos
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
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