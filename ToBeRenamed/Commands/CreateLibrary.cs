using MediatR;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Commands
{
    public class CreateLibrary : IRequest
    {
        public int UserId { get; }
        
        public LibraryDto Library { get; }

        public CreateLibrary(int userId, LibraryDto library)
        {
            UserId = userId;
            Library = library;
        }
    }
}