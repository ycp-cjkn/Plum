using MediatR;

namespace ToBeRenamed.Commands
{
    public class UpdateLibraryDesc : IRequest
    {
        public int MembershipId { get; }
        public string NewDesc { get; }


        public UpdateLibraryDesc(int membershipId, string newDesc)
        {
            MembershipId = membershipId;
            NewDesc = newDesc;
        }
    }
}