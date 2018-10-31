using System.Collections.Generic;
using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetAnnotationsByVideoId : IRequest<IEnumerable<AnnotationDto>>
    {
        public int VideoId { get; }

        public GetAnnotationsByVideoId(int videoId)
        {
            VideoId = videoId;
        }
    }
}