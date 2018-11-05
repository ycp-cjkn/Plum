using Dapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class ReplacePrivilegesOfRoleHandler : IRequestHandler<ReplacePrivilegesOfRole>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public ReplacePrivilegesOfRoleHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(ReplacePrivilegesOfRole request, CancellationToken cancellationToken)
        {
            const string deleteSql = @"
                DELETE FROM plum.role_privileges
                WHERE role_id = @RoleId";

            const string insertSql = @"
                INSERT INTO plum.role_privileges (role_id, privilege_alias)
                VALUES (@RoleId, @Alias)
                ON CONFLICT DO NOTHING";

            var rps = request.Privileges.Distinct().Select(p => new { p.Alias, request.RoleId });

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var cnn = _sqlConnectionFactory.GetSqlConnection())
                {
                    await cnn.ExecuteAsync(deleteSql, request);
                    await cnn.ExecuteAsync(insertSql, rps);
                }

                scope.Complete();
            }

            return Unit.Value;
        }
    }
}
