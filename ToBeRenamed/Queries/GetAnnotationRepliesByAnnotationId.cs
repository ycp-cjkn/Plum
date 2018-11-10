using System.Collections.Generic;
using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetAnnotationRepliesByAnnotationId : IRequest<IEnumerable<ReplyDto>>
    {
        public int AnnotationId { get; }

        public GetAnnotationRepliesByAnnotationId(int annotationId)
        {
            AnnotationId = annotationId;
        }
    }
}