using MediatR;

namespace ToBeRenamed.Commands
{
    public class DeleteRoleById : IRequest
    {
        public int Id { get; }

        public DeleteRoleById(int id)
        {
            Id = id;
        }
    }
}