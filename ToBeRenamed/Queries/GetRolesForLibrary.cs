using MediatR;
using System.Collections.Generic;
using ToBeRenamed.Models;

namespace ToBeRenamed.Queries
{
    public class GetRolesForLibrary : IRequest<IEnumerable<Role>>
    {
        public int LibraryId { get; }

        public GetRolesForLibrary(int libraryId)
        {
            LibraryId = libraryId;
        }
    }
}
