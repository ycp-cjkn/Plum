using MediatR;

namespace ToBeRenamed.Commands
{
    public class UpdateLibraryTitle : IRequest
    {
        public int LibraryId { get; }
        public string NewTitle { get; }


        public UpdateLibraryTitle(int libraryId, string newTitle)
        {
            LibraryId = libraryId;
            NewTitle = newTitle;
        }
    }
}