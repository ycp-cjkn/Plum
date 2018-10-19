using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetMembershipDto : IRequest<MembershipDto>
    {
        public int UserId { get; }
        public int LibraryId { get; }

        public GetMembershipDto(int userId, int libraryId)
        {
            UserId = userId;
            LibraryId = libraryId;
        }
    }
}