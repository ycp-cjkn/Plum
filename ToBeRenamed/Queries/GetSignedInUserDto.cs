using System.Security.Claims;
using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetSignedInUserDto : IRequest<UserDto>
    {
        public ClaimsPrincipal User { get; }

        public GetSignedInUserDto(ClaimsPrincipal user)
        {
            User = user;
        }
    }
}
