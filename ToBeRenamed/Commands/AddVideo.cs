using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Commands
{
    public class AddVideo : IRequest
    {
        public int UserId { get; }
        public int LibraryId { get; }
        public string Title { get; }
        public string Link { get; }
        public string Description { get; }


        public AddVideo(int userId, int libraryId, string title, string link, string description)
        {
            UserId = userId;
            LibraryId = libraryId;
            Title = title;
            Link = link;
            Description = description;
        }
    }
}