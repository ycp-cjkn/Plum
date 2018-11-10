using MediatR;

namespace ToBeRenamed.Commands
{
    public class DeleteVideoFromLibrary : IRequest
    {
        public int DeletedAt;
        public int VideoId;

        public DeleteVideoFromLibrary(int deletedAt, int videoId)
        {
            DeletedAt = deletedAt;
            VideoId = videoId;
        }
    }
}
