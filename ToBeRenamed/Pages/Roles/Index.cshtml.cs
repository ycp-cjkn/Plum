using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Dtos;
using ToBeRenamed.Models;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Pages.Roles
{
    public class IndexModel : PageModel
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public IndexModel(IMapper mapper, IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public IEnumerable<MemberDto> Members;
        public LibraryDto Library;
        public IEnumerable<Role> Roles;

        public async Task OnGetAsync(int libraryId)
        {
            Members = await _mediator.Send(new GetMembersOfLibrary(libraryId));
            Library = await _mediator.Send(new GetLibraryDtoById(libraryId));
            Roles = await _mediator.Send(new GetRolesForLibrary(libraryId));
        }

        public async Task<IActionResult> OnPostAsync(int roleId, string[] privileges)
        {
            var set = _mapper.Map<ISet<Privilege>>(privileges);

            await _mediator.Send(new ReplacePrivilegesOfRole(roleId, set));

            return RedirectToPage();
        }
    }
}