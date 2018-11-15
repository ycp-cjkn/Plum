using MediatR;
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

        public InvitationDto Invitation { get; set; }

        public async Task OnGetAsync(string urlKey)
        {
            Invitation = await _mediator.Send(new GetInvitationByKey(urlKey));
        }
    }
}