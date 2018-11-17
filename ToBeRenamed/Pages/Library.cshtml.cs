using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Dtos;
using ToBeRenamed.Extensions;
using ToBeRenamed.Models;
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
        public IEnumerable<Member> Members { get; set; }
        public IEnumerable<VideoDto> Videos { get; set; }
        public Member Member { get; set; }

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

        [BindProperty(SupportsGet = true)]
        public int LibraryId { get; set; }

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

        public async Task<IActionResult> OnPostDeleteVideo(int videoId)
        {
            var member = await _mediator.Send(new GetSignedInMember(User, LibraryId));

            if (!member.Role.Privileges.Contains(Privilege.CanRemoveAnyVideo))
            {
                return this.InsufficientPrivileges();
            }

            await _mediator.Send(new DeleteVideoFromLibrary(videoId));
            return RedirectToPage();
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

        // TODO: Don't do it this way
        private async Task SetUpPage()
        {
            var libraryTask = _mediator.Send(new GetLibraryDtoById(Id));
            var membersTask = _mediator.Send(new GetMembersOfLibrary(Id));
            var videosTask = _mediator.Send(new GetVideosOfLibrary(Id));
            var memberTask = _mediator.Send(new GetSignedInMember(User, Id));

            Library = await libraryTask.ConfigureAwait(false);
            Members = await membersTask.ConfigureAwait(false);
            Videos = await videosTask.ConfigureAwait(false);
            Member = await memberTask.ConfigureAwait(false);
        }
    }
}