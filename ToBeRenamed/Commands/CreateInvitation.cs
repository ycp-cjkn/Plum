using MediatR;
using System;

namespace ToBeRenamed.Commands
{
    public class CreateInvitation : IRequest
    {
        public int RoleId { get; }
        public int CreatedBy { get; }
        public DateTime? ExpiresAt { get; }

        public CreateInvitation(int roleId, int createdBy, DateTime? expiresAt)
        {
            RoleId = roleId;
            CreatedBy = createdBy;
            ExpiresAt = expiresAt;
        }
    }
}
