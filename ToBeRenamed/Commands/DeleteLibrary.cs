using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToBeRenamed.Commands
{
    public class DeleteLibrary : IRequest
    {
        public int LibraryId;

        public DeleteLibrary(int libraryId)
        {
            LibraryId = libraryId;
        }
    }
}
