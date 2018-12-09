using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Commands
{
    public class CreateTag : IRequest<TagDto>
    {

        public string Tag { get; }

        public CreateTag(string tag)
        {
            Tag = tag;
        }
    }
}
