using MediatR;
using System.Collections.Generic;
using ToBeRenamed.Models;

namespace ToBeRenamed.Queries
{
    public class GetMembersOfLibrary : IRequest<IEnumerable<Member>>
    {
        public int LibraryId { get; }

        public GetMembersOfLibrary(int libraryId)
        {
            LibraryId = libraryId;
        }
    }
}
