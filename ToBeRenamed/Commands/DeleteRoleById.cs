using MediatR;

namespace ToBeRenamed.Commands
{
    public class DeleteRoleById : IRequest
    {
        public int RoleId { get; }

        public DeleteRoleById(int roleId)
        {
            RoleId = roleId;
        }
    }
}