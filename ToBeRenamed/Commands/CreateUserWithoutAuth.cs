using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Commands
{
    public class CreateUserWithoutAuth : IRequest<UserDto>
    {
        public string DisplayName { get; }

        public CreateUserWithoutAuth(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
