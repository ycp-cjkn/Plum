using System.Collections.Generic;
using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetAnnotationRepliesByVideoId : IRequest<IEnumerable<ReplyDto>>
    {
        public int VideoId { get; }

        public GetAnnotationRepliesByVideoId(int videoId)
        {
            VideoId = videoId;
        }
    }
}