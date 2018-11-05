using Dapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class AddPrivilegesToRoleHandler : IRequestHandler<AddPrivilegesToRole>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public AddPrivilegesToRoleHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(AddPrivilegesToRole request, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO plum.role_privileges (role_id, privilege_alias)
                VALUES (@RoleId, @Alias)
                ON CONFLICT DO NOTHING";

            var rps = request.Privileges.Distinct().Select(p => new { p.Alias, request.RoleId });

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var cnn = _sqlConnectionFactory.GetSqlConnection())
                {
                    await cnn.ExecuteAsync(sql, rps);
                }

                scope.Complete();
            }

            return Unit.Value;
        }
    }
}
