using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Dtos;
using ToBeRenamed.Models;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages.Invitations
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty(SupportsGet = true)]
        public int LibraryId { get; set; }

        public LibraryDto Library { get; set; }
        public Member Member { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<InvitationDto> Invitations { get; set; }

        public async Task OnGetAsync()
        {
            await SetUp();
        }

        public async Task<IActionResult> OnPostInviteUserAsync(InviteUserRequest request)
        {
            await SetUp();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var create = new CreateInvitation(request.RoleId, Member.Id, request.ExpiresAt);
            await _mediator.Send(create);

            return RedirectToPage();
        }

        private async Task SetUp()
        {
            var libraryTask = _mediator.Send(new GetLibraryDtoById(LibraryId));
            var memberTask = _mediator.Send(new GetSignedInMember(User, LibraryId));
            var rolesTask = _mediator.Send(new GetRolesForLibrary(LibraryId));
            var invitationsTask = _mediator.Send(new GetInvitationsForLibrary(LibraryId));

            Library = await libraryTask.ConfigureAwait(false);
            Member = await memberTask.ConfigureAwait(false);
            Roles = await rolesTask.ConfigureAwait(false);
            Invitations = await invitationsTask.ConfigureAwait(false);
        }

        public enum Expiration { OneHour, OneDay, SevenDays, ThirtyDays, Never };

        public class InviteUserRequest
        {
            [Required]
            public int RoleId { get; set; }

            [Required]
            public Expiration Expiration { get; set; }

            public DateTime? ExpiresAt
            {
                get
                {
                    switch (Expiration)
                    {
                        case Expiration.OneHour:
                            return DateTime.Now.AddHours(1);
                        case Expiration.OneDay:
                            return DateTime.Now.AddDays(1);
                        case Expiration.SevenDays:
                            return DateTime.Now.AddDays(7);
                        case Expiration.ThirtyDays:
                            return DateTime.Now.AddDays(30);
                        case Expiration.Never:
                            return null;
                        default:
                            return null;
                    }
                }
            }
        }
    }
}