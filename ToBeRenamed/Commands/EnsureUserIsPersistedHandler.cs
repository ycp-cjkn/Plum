using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
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
            var name = ClaimEndsWith(request.User.Claims, "/name");
            var nameidentifier = ClaimEndsWith(request.User.Claims, "/nameidentifier");

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

        private static string ClaimEndsWith(IEnumerable<Claim> claims, string endsWith)
        {
            return claims.FirstOrDefault(c => c.Type.EndsWith(endsWith))?.Value;
        }
    }
}
