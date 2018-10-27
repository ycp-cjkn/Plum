using System.Collections.Generic;
using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetAnnotationsByVideoId : IRequest<IEnumerable<AnnotationDto>>
    {
        public int Id { get; }

        public GetAnnotationsByVideoId(int id)
        {
            Id = id;
        }
    }
}