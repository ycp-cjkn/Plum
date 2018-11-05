using MediatR;

namespace ToBeRenamed.Commands
{
    public class UpdateLibraryTitle : IRequest
    {
        public int MembershipId { get; }
        public string NewTitle { get; }


        public UpdateLibraryTitle(int membershipId, string newTitle)
        {
            MembershipId = membershipId;
            NewTitle = newTitle;
        }
    }
}