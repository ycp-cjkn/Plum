using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using ToBeRenamed.Attributes;
using ToBeRenamed.Commands;
using ToBeRenamed.Models;
using ToBeRenamed.Queries;
using ToBeRenamed.Extensions;

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
        [Required]
        public string Title { get; set; }

        [BindProperty]
        [Required]
        [YoutubeLink]
        public string Link { get; set; }

        [BindProperty]
        public string Description { get; set; }

        [BindProperty(SupportsGet = true)]
        public int LibraryId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var member = await _mediator.Send(new GetSignedInMember(User, LibraryId));

            if (!member.Role.Privileges.Contains(Privilege.CanSubmitVideo))
            {
                return this.InsufficientPrivileges();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            var member = await _mediator.Send(new GetSignedInMember(User, LibraryId));

            if (!member.Role.Privileges.Contains(Privilege.CanSubmitVideo))
            {
                return this.InsufficientPrivileges();
            }

            await _mediator.Send(new CreateVideo(member.UserId, LibraryId, Title, Link, Description));

            return Redirect($"/Library/{LibraryId}");
        }
    }
}