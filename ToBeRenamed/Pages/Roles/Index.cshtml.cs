using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Models;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages.Roles
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;

        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IEnumerable<Role> Roles;

        public async Task OnGetAsync(int libraryId)
        {
            Roles = await _mediator.Send(new GetRolesForLibrary(libraryId));
        }
    }
}