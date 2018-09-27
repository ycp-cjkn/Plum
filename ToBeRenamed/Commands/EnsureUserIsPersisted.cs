using MediatR;
using System.Security.Claims;

namespace ToBeRenamed.Commands
{
    public class EnsureUserIsPersisted : IRequest
    {
        public ClaimsPrincipal User { get;}

        public EnsureUserIsPersisted(ClaimsPrincipal user)
        {
            User = user;
        }
    }
}
