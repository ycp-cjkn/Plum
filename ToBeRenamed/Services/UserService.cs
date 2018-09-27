using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Services
{
    public class UserService
    {
        private readonly SqlConnectionFactory _sqlConnectionFactory;

        public UserService(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public void EnsureUserIsPersisted(ClaimsPrincipal user)
        {
            var name = ClaimEndsWith(user.Claims, "/name");
            var nameidentifier = ClaimEndsWith(user.Claims, "/nameidentifier");

            const string sql = @"
                INSERT INTO plum.users (display_name, google_claim_nameidentifier)
                VALUES (@name, @nameidentifier)
                ON CONFLICT ON CONSTRAINT users_google_claim_nameidentifier_key DO NOTHING";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                cnn.Execute(sql, new { name, nameidentifier });
            }
        }

        private static string ClaimEndsWith(IEnumerable<Claim> claims, string endsWith)
        {
            return claims.FirstOrDefault(c => c.Type.EndsWith(endsWith))?.Value;
        }
    }
}
