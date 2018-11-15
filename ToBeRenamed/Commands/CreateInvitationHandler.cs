using Dapper;
using MediatR;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class CreateInvitationHandler : IRequestHandler<CreateInvitation>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CreateInvitationHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(CreateInvitation request, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO plum.invitations (url_key, role_id, membership_id, expires_at)
                VALUES (@UrlKey, @RoleId, @MembershipId, @ExpiresAt)";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, new
                {
                    UrlKey = GenerateUrlKey(),
                    request.RoleId,
                    request.MembershipId,
                    request.ExpiresAt
                });
            }

            return Unit.Value;
        }

        private static string GenerateUrlKey()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";

            var random = new Random();
            var builder = new StringBuilder();

            for (var i = 0; i < 7; i++)
            {
                builder.Append(chars[random.Next(chars.Length)]);
            }

            return builder.ToString();
        }
    }
}
