using System.Collections.Generic;
using MediatR;
using ToBeRenamed.Models;

namespace ToBeRenamed.Queries
{
    public class GetRolesForMembers : IRequest<IDictionary<int, Role>>
    {
        public IEnumerable<int> MembershipIds { get; }

        public GetRolesForMembers(IEnumerable<int> membershipIds)
        {
            MembershipIds = membershipIds;
        }
    }
}
