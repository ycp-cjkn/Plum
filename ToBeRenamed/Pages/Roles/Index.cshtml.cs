using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public IEnumerable<Member> Members;
        public LibraryDto Library;
        public IEnumerable<Role> Roles;

        [BindProperty(SupportsGet = true)]
        public int LibraryId { get; set; }

        [Required]
        [MaxLength(16)]
        [BindProperty]
        public string NewRoleTitle { get; set; }

        public async Task OnGetAsync()
        {
            Members = await _mediator.Send(new GetMembersOfLibrary(LibraryId));
            Library = await _mediator.Send(new GetLibraryDtoById(LibraryId));
            Roles = await _mediator.Send(new GetRolesForLibrary(LibraryId));
        }

        public async Task<IActionResult> OnPostDeleteRoleAsync(int roleId)
        {
            await _mediator.Send(new DeleteRoleById(roleId));
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdatePrivilegesAsync(int roleId, string[] privileges)
        {
            var set = _mapper.Map<ISet<Privilege>>(privileges);
            await _mediator.Send(new ReplacePrivilegesOfRole(roleId, set));

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCreateRoleAsync()
        {
            if (ModelState.IsValid)
            {
                // TODO: Should only be able to create a role if the user has the
                // privilege and is a member of the Library
                await _mediator.Send(new CreateRole(NewRoleTitle, LibraryId));
            }

            return RedirectToPage();
        }
    }
}