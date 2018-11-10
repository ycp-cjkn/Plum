using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class UpdateRoleOfMemberHandler : IRequestHandler<UpdateRoleOfMember>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public UpdateRoleOfMemberHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(UpdateRoleOfMember request, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE plum.memberships
                SET role_id = @RoleId
                WHERE id = @MemberId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.QueryAsync(sql, request);
            }

            return Unit.Value;
        }
    }
}
