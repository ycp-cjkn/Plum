using MediatR;

namespace ToBeRenamed.Commands
{
    public class CreateRole : IRequest<int>
    {
        public string Title { get; }
        public int LibraryId { get; }

        public CreateRole(string title, int libraryId)
        {
            Title = title;
            LibraryId = libraryId;
        }
    }
}
