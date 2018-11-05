using MediatR;

namespace ToBeRenamed.Commands
{
    public class UpdateLibraryInfo : IRequest
    {
        public int LibraryId { get; }
        public string NewTitle { get; }
        public string NewDescription { get; }

        public UpdateLibraryInfo(int libraryId, string newTitle, string newDescription)
        {
            LibraryId = libraryId;
            NewTitle = newTitle;
            NewDescription = newDescription;

        }
    }
}