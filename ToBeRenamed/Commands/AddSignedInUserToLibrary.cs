using MediatR;
using System.Security.Claims;

namespace ToBeRenamed.Commands
{
    public class AddSignedInUserToLibrary : IRequest
    {
        public ClaimsPrincipal User { get; }
        public int LibraryId { get; }
        public int RoleId { get; set; }

        public AddSignedInUserToLibrary(ClaimsPrincipal user, int libraryId, int roleId)
        {
            User = user;
            LibraryId = libraryId;
            RoleId = roleId;
        }
    }
}
