using MediatR;

namespace ToBeRenamed.Commands
{
    public class UpdateRoleOfMember : IRequest
    {
        public int MemberId { get; }
        public int RoleId { get; }
        public string DisplayName { get; }

        public UpdateRoleOfMember(int memberId, int roleId, string displayName)
        {
            MemberId = memberId;
            RoleId = roleId;
            DisplayName = displayName;
        }
    }
}