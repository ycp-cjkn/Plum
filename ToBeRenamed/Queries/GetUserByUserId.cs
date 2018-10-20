using System.Security.Claims;
using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetUserByUserId : IRequest<UserDto>
    {
        public int UserId { get; }

        public GetUserByUserId(int userId)
        {
            UserId = userId;
        }
    }
}
