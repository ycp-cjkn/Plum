using MediatR;
using System;

namespace ToBeRenamed.Commands
{
    public class CreateInvitation : IRequest
    {
        public int RoleId { get; }
        public int MembershipId { get; }
        public DateTime? ExpiresAt { get; }

        public CreateInvitation(int roleId, int membershipId, DateTime? expiresAt)
        {
            RoleId = roleId;
            MembershipId = membershipId;
            ExpiresAt = expiresAt;
        }
    }
}
