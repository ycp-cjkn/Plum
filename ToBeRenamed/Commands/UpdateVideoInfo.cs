using MediatR;

namespace ToBeRenamed.Commands
{
    public class UpdateVideoInfo : IRequest
    {
        public int VideoId { get; }
        public string NewTitle { get; }
        public string NewDescription { get; }

        public UpdateVideoInfo(int videoId, string newTitle, string newDescription)
        {
            VideoId = videoId;
            NewTitle = newTitle;
            NewDescription = newDescription;

        }
    }
}