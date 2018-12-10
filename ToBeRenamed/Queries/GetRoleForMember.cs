using MediatR;
using ToBeRenamed.Models;

namespace ToBeRenamed.Queries
{
    public class GetRoleForMember : IRequest<Role>
    {
        public int MembershipId { get; }

        public GetRoleForMember(int membershipId)
        {
            MembershipId = membershipId;
        }
    }
}
