using System.Collections.Generic;
using MediatR;
using ToBeRenamed.Models;

namespace ToBeRenamed.Queries
{
    public class GetRolesForMembers : IRequest<IDictionary<int, Role>>
    {
        public IList<int> MembershipIds { get; }

        public GetRolesForMembers(IList<int> membershipIds)
        {
            MembershipIds = membershipIds;
        }
    }
}
