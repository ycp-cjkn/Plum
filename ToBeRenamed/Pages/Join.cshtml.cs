using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages
{
    public class JoinModel : PageModel
    {
        private readonly IMediator _mediator;

        public JoinModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        [BindProperty(SupportsGet = true)]
        public string UrlKey { get; set; }

        public InvitationDto Invitation { get; set; }

        public async Task OnGetAsync()
        {
            Invitation = await _mediator.Send(new GetInvitationByKey(UrlKey));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Invitation = await _mediator.Send(new GetInvitationByKey(UrlKey));

            return RedirectToPage("/Library", "AcceptInvitation", new
            {
                id = Invitation.LibraryId,
                urlKey = UrlKey
            });
        }
    }
}