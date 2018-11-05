using MediatR;

namespace ToBeRenamed.Commands
{
    public class UpdateLibraryDesc : IRequest
    {
        public int LibraryId { get; }
        public string NewDesc { get; }


        public UpdateLibraryDesc(int libraryId, string newDesc)
        {
            LibraryId = libraryId;
            NewDesc = newDesc;
        }
    }
}