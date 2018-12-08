using System.Collections.Generic;
using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetInvitationsForLibrary : IRequest<IEnumerable<InvitationDto>>
    {
        public int LibraryId { get; }

        public GetInvitationsForLibrary(int libraryId)
        {
            LibraryId = libraryId;
        }
    }
}