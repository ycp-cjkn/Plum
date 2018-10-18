using MediatR;
using System.Collections.Generic;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetMembersOfLibrary : IRequest<IEnumerable<MemberDto>>
    {
        public int LibraryId { get;}

        public GetMembersOfLibrary(int libraryId)
        {
            LibraryId = libraryId;
        }
    }
}
