using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Commands
{
    public class AddVideo : IRequest
    {
        public int UserId { get; }
        public string Title { get; }
        public string Link { get; }
        public string Description { get; }


        public AddVideo(int userId, string title, string link, string description)
        {
            UserId = userId;
            Title = title;
            Link = link;
            Description = description;
        }
    }
}