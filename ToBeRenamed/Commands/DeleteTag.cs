using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Commands
{
    public class DeleteTag : IRequest
    {

        public int Id { get; }
        public string Tag { get; }

        public DeleteTag(int id, string tag)
        {
            Id = id;
            Tag = tag;
        }
    }
}
