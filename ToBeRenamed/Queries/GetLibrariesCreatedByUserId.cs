using MediatR;
using System.Collections.Generic;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetLibrariesCreatedByUserId : IRequest<IEnumerable<LibraryDto>>
    {
        public int UserId { get; }

        public GetLibrariesCreatedByUserId(int userId)
        {
            UserId = userId;
        }
    }
}
