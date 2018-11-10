using MediatR;

namespace ToBeRenamed.Commands
{
    public class UpdateRoleOfMember : IRequest
    {
        public int MemberId { get; }
        public int RoleId { get; }

        public UpdateRoleOfMember(int memberId, int roleId)
        {
            MemberId = memberId;
            RoleId = roleId;
        }
    }
}