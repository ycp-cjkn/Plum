using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Dtos;
using ToBeRenamed.Extensions;
using ToBeRenamed.Models;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages.Videos
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IMediator _mediator;

        public EditModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public VideoDto Video { get; set; }
        public Member Member { get; set; }

        [BindProperty]
        [Required]
        public string NewTitle { get; set; }

        [BindProperty]
        [Required]
        public string NewDescription { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Video = await _mediator.Send(new GetVideoById(id));
            Member = await _mediator.Send(new GetSignedInMember(User, Video.LibraryId));

            if (!Member.Role.Privileges.Contains(Privilege.CanEditVideo))
            {
                return this.InsufficientPrivileges();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Video = await _mediator.Send(new GetVideoById(id));
            Member = await _mediator.Send(new GetSignedInMember(User, Video.LibraryId));

            if (!Member.Role.Privileges.Contains(Privilege.CanEditVideo))
            {
                return this.InsufficientPrivileges();
            }

            await _mediator.Send(new UpdateVideoInfo(id, NewTitle, NewDescription));

            return RedirectToPage("/Videos/Index");
        }
    }
}