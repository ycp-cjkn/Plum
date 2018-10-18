using System.Collections.Generic;
using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetLibrariesForUser : IRequest<IEnumerable<LibraryDto>>
    {
        public int UserId { get; }

        public GetLibrariesForUser(int userId)
        {
            UserId = userId;
        }
    }
}
