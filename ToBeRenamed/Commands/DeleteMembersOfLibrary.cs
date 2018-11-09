using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToBeRenamed.Commands
{
    public class DeleteMembersOfLibrary : IRequest
    {
        public bool IsDeleted;
        public int UserId;

        public DeleteMembersOfLibrary(bool isDeleted, int userId)
        {
            IsDeleted = isDeleted;
            UserId = userId;
        }
    }
}
