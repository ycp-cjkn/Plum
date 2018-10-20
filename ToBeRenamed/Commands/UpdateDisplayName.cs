using MediatR;

namespace ToBeRenamed.Commands
{
    public class UpdateDisplayName : IRequest
    {
        public int MembershipId { get; }
        public string NewDisplayName { get; }


        public UpdateDisplayName(int membershipId, string newDisplayName)
        {
            MembershipId = membershipId;
            NewDisplayName = newDisplayName;
        }
    }
}