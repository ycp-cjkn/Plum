using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class UpdateDisplayNameHandler : IRequestHandler<UpdateDisplayName>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public UpdateDisplayNameHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(UpdateDisplayName request, CancellationToken cancellationToken)
        {
            var newDisplayName = request.NewDisplayName;
            var membershipId = request.MembershipId;

            const string updateDisplayNameSql = @"
                UPDATE plum.memberships
                SET display_name = @newDisplayName
                WHERE id = @membershipId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.QueryAsync(updateDisplayNameSql, new { newDisplayName, membershipId });
            }

            return Unit.Value;
        }
    }
}
