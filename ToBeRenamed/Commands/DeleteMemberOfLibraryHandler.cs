using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class DeleteMemberOfLibraryHandler : IRequestHandler<DeleteMemberOfLibrary>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public DeleteMemberOfLibraryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(DeleteMemberOfLibrary request, CancellationToken cancellationToken)
        {
            const string sql = @"
                DELETE FROM plum.memberships
                WHERE id = @MembershipId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.QueryAsync(sql, request);
            }

            return Unit.Value;
        }
    }
}
