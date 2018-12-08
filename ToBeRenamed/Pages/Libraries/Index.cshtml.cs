using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using ToBeRenamed.Dtos;
using ToBeRenamed.Queries;
using Microsoft.AspNetCore.Mvc;
using ToBeRenamed.Models;
using ToBeRenamed.Extensions;
using ToBeRenamed.Commands;

namespace ToBeRenamed.Pages.Libraries
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty(SupportsGet = true)]
        public int LibraryId { get; set; }

        public IEnumerable<LibraryDto> Libraries { get; set; }

        public async Task OnGet()
        {
            var userDto = await _mediator.Send(new GetSignedInUserDto(User));
            Libraries = await _mediator.Send(new GetLibrariesForUser(userDto.Id));
        }

        public async Task<IActionResult> OnPostDeleteLibrary(int libraryId)
        {
            var member = await _mediator.Send(new GetSignedInMember(User, LibraryId));

            if (!member.Role.Privileges.Contains(Privilege.CanRemoveLibrary))
            {
                return this.InsufficientPrivileges();
            }

            await _mediator.Send(new DeleteLibrary(libraryId));
            return RedirectToPage();
        }
    }
}