using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Commands
{
    public class CreateTag : IRequest
    {

        public string Tag { get; }

        public CreateTag(string tag)
        {
            Tag = tag;
        }
    }
}
