using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class DeleteMembersOfLibraryHandler : IRequestHandler<DeleteMembersOfLibrary>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public DeleteMembersOfLibraryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(DeleteMembersOfLibrary request, CancellationToken cancellationToken)
        {
            var isDeleted= request.IsDeleted;
            var userId = request.UserId;

            const string DeleteMemberOfLibrarySql = @"
                UPDATE plum.memberships
                SET is_deleted = @isDeleted
                WHERE id = @userId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                {
                    await cnn.QueryAsync(DeleteMemberOfLibrarySql, new { isDeleted, userId });
                }
            }
            return Unit.Value;
        }
    }
}
