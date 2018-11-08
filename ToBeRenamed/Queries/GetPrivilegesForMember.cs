using MediatR;
using System.Collections.Generic;
using ToBeRenamed.Models;

namespace ToBeRenamed.Queries
{
    public class GetPrivilegesForMember : IRequest<ISet<Privilege>>
    {
        public int MembershipId { get; }

        public GetPrivilegesForMember(int membershipId)
        {
            MembershipId = membershipId;
        }
    }
}
