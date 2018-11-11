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
using ToBeRenamed.Extensions;
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

        public LibraryDto Library;
        public Member Member;
        public IEnumerable<Member> Members;
        public IEnumerable<Role> Roles;

        [Required]
        [BindProperty(SupportsGet = true)]
        public int LibraryId { get; set; }

        private const string PrivilegeError = "At least one member must have the ability to access this page";

        public async Task<IActionResult> OnGetAsync()
        {
            await SetUp();

            if (!Member.Role.Privileges.Contains(Privilege.CanManageMembersAndRoles))
            {
                return this.InsufficientPrivileges();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostCreateRoleAsync(CreateRoleRequest request)
        {
            await SetUp();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!Member.Role.Privileges.Contains(Privilege.CanManageMembersAndRoles))
            {
                return this.InsufficientPrivileges();
            }

            await _mediator.Send(new CreateRole(request.Title, LibraryId));
            return RedirectToPage();
        }

        public async Task<IActionResult> OnGetDeleteMemberAsync(DeleteMemberRequest request)
        {
            await SetUp();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!Member.Role.Privileges.Contains(Privilege.CanManageMembersAndRoles))
            {
                return this.InsufficientPrivileges();
            }

            if (request.MembershipId == Member.Id)
            {
                return this.InsufficientPrivileges();
            }

            await _mediator.Send(_mapper.Map<DeleteMemberOfLibrary>(request));
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteRoleAsync(DeleteRoleRequest request)
        {
            await SetUp();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!Member.Role.Privileges.Contains(Privilege.CanManageMembersAndRoles))
            {
                return this.InsufficientPrivileges();
            }

            if (Members.Any(m => m.Role.Id == request.RoleId))
            {
                return this.InsufficientPrivileges();
            }

            await _mediator.Send(_mapper.Map<DeleteRoleById>(request));
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateMembersAsync(UpdateMemberRequest[] requests)
        {
            await SetUp();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!Member.Role.Privileges.Contains(Privilege.CanManageMembersAndRoles))
            {
                return this.InsufficientPrivileges();
            }

            // Ensure that at least one role (with at least 1 member) can manage members and roles
            var memberWithGivenPrivilegeExists = false;

            foreach (var roleId in requests.Select(r => r.RoleId))
            {
                var role = Roles.Single(r => r.Id == roleId);

                if (role.Privileges.Contains(Privilege.CanManageMembersAndRoles))
                {
                    memberWithGivenPrivilegeExists = true;
                    break;
                }
            }

            if (!memberWithGivenPrivilegeExists)
            {
                ModelState.AddModelError(string.Empty, PrivilegeError);
                return Page();
            }

            var updates = _mapper.Map<IEnumerable<UpdateRoleOfMember>>(requests);

            var tasks = updates.Select(u => _mediator.Send(u));

            await Task.WhenAll(tasks);

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostUpdateRoleAsync(UpdateRoleRequest request)
        {
            await SetUp();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!Member.Role.Privileges.Contains(Privilege.CanManageMembersAndRoles))
            {
                return this.InsufficientPrivileges();
            }

            var privileges = _mapper.Map<ISet<Privilege>>(request.Privileges ?? Enumerable.Empty<string>());

            // Ensure that at least one role (with at least 1 member) can manage members and roles
            var memberWithGivenPrivilegeExists = false;

            foreach (var role in Members.Select(m => m.Role))
            {
                // Simulate replacing privileges for given role
                if (role.Id == request.RoleId)
                {
                    role.Privileges = privileges;
                }

                if (role.Privileges.Contains(Privilege.CanManageMembersAndRoles))
                {
                    memberWithGivenPrivilegeExists = true;
                    break;
                }
            }

            if (!memberWithGivenPrivilegeExists)
            {
                ModelState.AddModelError(string.Empty, PrivilegeError);
                return Page();
            }

            await _mediator.Send(new ReplacePrivilegesOfRole(request.RoleId, privileges));
            return RedirectToPage();
        }

        private async Task SetUp()
        {
            var memberTask = _mediator.Send(new GetSignedInMember(User, LibraryId));
            var membersTask = _mediator.Send(new GetMembersOfLibrary(LibraryId));
            var libraryTask = _mediator.Send(new GetLibraryDtoById(LibraryId));
            var rolesTask = _mediator.Send(new GetRolesForLibrary(LibraryId));

            Member = await memberTask.ConfigureAwait(false);
            Members = await membersTask.ConfigureAwait(false);
            Library = await libraryTask.ConfigureAwait(false);
            Roles = await rolesTask.ConfigureAwait(false);
        }

        public class CreateRoleRequest
        {
            [Required]
            [MaxLength(16)]
            public string Title { get; set; }
        }

        public class DeleteRoleRequest
        {
            [Required]
            public int RoleId { get; set; }
        }

        public class DeleteMemberRequest
        {
            [Required]
            public int MembershipId { get; set; }
        }

        public class UpdateMemberRequest
        {
            private string _displayName;

            [Required]
            public int MemberId { get; set; }

            [Required]
            public int RoleId { get; set; }

            public string DisplayName
            {
                get => _displayName;
                set => _displayName = value ?? string.Empty;
            }
        }

        public class UpdateRoleRequest
        {
            [Required]
            public int RoleId { get; set; }

            public IEnumerable<string> Privileges { get; set; }
        }
    }
}