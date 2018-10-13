using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class ChangeDisplayNameHandler : IRequestHandler<ChangeDisplayName>
    {
        private readonly SqlConnectionFactory _sqlConnectionFactory;

        public ChangeDisplayNameHandler(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(ChangeDisplayName request, CancellationToken cancellationToken)
        {
            var newDisplayName = request.NewDisplayName;
            var membershipId = request.MembershipId;

            const string updateDisplayNameSql = @"
                UPDATE plum.memberships
                SET display_name = @newDisplayName
                WHERE id = @membershipId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.QueryAsync(updateDisplayNameSql, new {newDisplayName, membershipId});
            }

            return Unit.Value;
        }
    }
}
