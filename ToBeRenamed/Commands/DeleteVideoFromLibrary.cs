using MediatR;

namespace ToBeRenamed.Commands
{
    public class DeleteVideoFromLibrary : IRequest
    {
        public int VideoId;

        public DeleteVideoFromLibrary(int videoId)
        {
            VideoId = videoId;
        }
    }
}
