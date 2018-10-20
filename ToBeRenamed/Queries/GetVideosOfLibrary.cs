using System.Collections.Generic;
using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetVideosOfLibrary: IRequest<IEnumerable<VideoDto>>
    {
        public int LibraryId { get; }

        public GetVideosOfLibrary(int libraryId)
        {
            LibraryId = libraryId;
        }
    }
}
