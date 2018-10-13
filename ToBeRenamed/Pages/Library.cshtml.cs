using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ToBeRenamed.Commands;
using ToBeRenamed.Dtos;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages
{
    public class LibraryModel : PageModel
    {
        private readonly IMediator _mediator;

        public LibraryModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public LibraryDto Library { get; set; }
        public IEnumerable<MemberDto> Members { get; set; }
        
        public MembershipDto Membership;

        public async Task OnGetAsync(int id)
        {
            var libraryTask = _mediator.Send(new GetLibraryDtoById(id));
            var userDto = await _mediator.Send(new GetSignedInUserDto(User));
            var membersTask = _mediator.Send(new GetMembersOfLibrary(id));

            Membership = await _mediator.Send(new GetMembershipDto(userDto.Id, Library.Id));
            Library = await libraryTask.ConfigureAwait(false);
            Members = await membersTask.ConfigureAwait(false);
        }

        [BindProperty]
        [StringLength(64, MinimumLength = 1)]
        [Display(Name = "Change Display Name")]
        [Required]
        public string NewDisplayName { get; set; }
        
        public async Task<IActionResult> OnPostDisplayNameAsync(string newDisplayName, int libraryId)
        {
            var userDto = await _mediator.Send(new GetSignedInUserDto(User));
            Membership = await _mediator.Send(new GetMembershipDto(userDto.Id, libraryId));
            
            await _mediator.Send(new UpdateDisplayName(Membership.Id, newDisplayName));

            return RedirectToPage();
        }
    }
}