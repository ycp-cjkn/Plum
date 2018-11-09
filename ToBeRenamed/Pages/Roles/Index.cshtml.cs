using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        [Required]
        [BindProperty(SupportsGet = true)]
        public int LibraryId { get; set; }

        public async Task OnGetAsync()
        {
            Members = await _mediator.Send(new GetMembersOfLibrary(LibraryId));
            Library = await _mediator.Send(new GetLibraryDtoById(LibraryId));
            Roles = await _mediator.Send(new GetRolesForLibrary(LibraryId));
        }

        public async Task<IActionResult> OnPostCreateRoleAsync(CreateRoleRequest request)
        {
            if (ModelState.IsValid)
            {
                // TODO: Should only be able to create a role if the user has the
                // privilege and is a member of the Library
                await _mediator.Send(new CreateRole(request.Title, LibraryId));
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteRoleAsync(DeleteRoleRequest request)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(_mapper.Map<DeleteRoleById>(request));
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateMembersAsync(UpdateMemberRequest[] requests)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage();
            }

            var updates = _mapper.Map<IEnumerable<UpdateRoleOfMember>>(requests);

            var tasks = updates.Select(u => _mediator.Send(u));

            await Task.WhenAll(tasks);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateRoleAsync(UpdateRoleRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage();
            }

            var set = _mapper.Map<ISet<Privilege>>(request.Privileges);
            await _mediator.Send(new ReplacePrivilegesOfRole(request.RoleId, set));

            return RedirectToPage();
        }

        public class CreateRoleRequest
        {
            [Required]
            [MaxLength(25)]
            public string Title { get; set; }
        }

        public class DeleteRoleRequest
        {
            [Required]
            public int RoleId { get; set; }
        }

        public class UpdateMemberRequest
        {
            [Required]
            public int MemberId { get; set; }

            [Required]
            public int RoleId { get; set; }

            public string DisplayName { get; set; }
        }

        public class UpdateRoleRequest
        {
            [Required]
            public int RoleId { get; set; }

            public IEnumerable<string> Privileges { get; set; }
        }
    }
}