using MediatR;
using ToBeRenamed.Dtos;
namespace ToBeRenamed.Queries
{
    public class GetInvitationByKey : IRequest<InvitationDto>
    {
        public string Key { get; }

        public GetInvitationByKey(string key)
        {
            Key = key;
        }
    }
}
