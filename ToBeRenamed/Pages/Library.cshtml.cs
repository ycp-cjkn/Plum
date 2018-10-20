using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
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
        public IEnumerable<VideoDto> Videos { get; set; }
        public MembershipDto Membership;

        [BindProperty]
        [StringLength(64, MinimumLength = 1)]
        [Display(Name = "Change Display Name")]
        [Required]
        public string NewDisplayName { get; set; }

        [BindProperty]
        [Required]
        public int MembershipId { get; set; }

        [BindProperty(SupportsGet = true)]
        [Required]
        public int Id { get; set; }

        public async Task OnGetAsync()
        {
            await SetUpPage();
        }

        public async Task<IActionResult> OnPostDisplayNameAsync()
        {
            if (!ModelState.IsValid)
            {
                await SetUpPage();
                return Page();
            }

            await _mediator.Send(new UpdateDisplayName(MembershipId, NewDisplayName));
            return RedirectToPage();
        }

        // TODO: Don't do it this way
        private async Task SetUpPage()
        {
            var libraryTask = _mediator.Send(new GetLibraryDtoById(Id));
            var membersTask = _mediator.Send(new GetMembersOfLibrary(Id));
            var userTask = _mediator.Send(new GetSignedInUserDto(User));
            var videosTask = _mediator.Send(new GetVideosOfLibrary(Id));

            Library = await libraryTask.ConfigureAwait(false);
            Members = await membersTask.ConfigureAwait(false);
            Videos = await videosTask.ConfigureAwait(false);

            var user = await userTask.ConfigureAwait(false);
            Membership = await _mediator.Send(new GetMembershipDto(user.Id, Library.Id));
        }
    }
}