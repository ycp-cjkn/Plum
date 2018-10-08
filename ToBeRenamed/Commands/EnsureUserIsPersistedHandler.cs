using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Extensions;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class EnsureUserIsPersistedHandler : IRequestHandler<EnsureUserIsPersisted>
    {
        private readonly SqlConnectionFactory _sqlConnectionFactory;

        public EnsureUserIsPersistedHandler(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(EnsureUserIsPersisted request, CancellationToken cancellationToken)
        {
            var name = request.User.GetName();
            var nameidentifier = request.User.GetNameIdentifier();

            const string sql = @"
                INSERT INTO plum.users (display_name, google_claim_nameidentifier)
                VALUES (@name, @nameidentifier)
                ON CONFLICT ON CONSTRAINT users_google_claim_nameidentifier_key DO NOTHING";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, new { name, nameidentifier });
            }

            return Unit.Value;
        }
    }
}
