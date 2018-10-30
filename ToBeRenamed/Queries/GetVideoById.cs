using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Queries
{
    public class GetVideoById : IRequest<VideoDto>
    {
        public int Id { get; }

        public GetVideoById(int id)
        {
            Id = id;
        }
    }
}