using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class DeleteRoleByIdHandler : IRequestHandler<DeleteRoleById>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public DeleteRoleByIdHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(DeleteRoleById request, CancellationToken cancellationToken)
        {
            const string sql = @"UPDATE plum.roles SET deleted_at = NOW() WHERE id = @Id";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, request);
            }

            return Unit.Value;
        }
    }
}
