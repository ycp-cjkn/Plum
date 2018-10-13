using MediatR;

namespace ToBeRenamed.Commands
{
    public class ChangeDisplayName : IRequest
    {
        public int MembershipId { get; }
        public string NewDisplayName { get; }


        public ChangeDisplayName(int membershipId, string newDisplayName)
        {
            MembershipId = membershipId;
            NewDisplayName = newDisplayName;
        }
    }
}