using Dapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Extensions;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Queries
{
    public class GetUserByUserIdHandler : IRequestHandler<GetUserByUserId, UserDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetUserByUserIdHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<UserDto> Handle(GetUserByUserId request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT id, display_name, google_claim_nameidentifier, created_at
                FROM plum.users
                WHERE id = @UserId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                var users = await cnn.QueryAsync<UserDto>(sql, new { request.UserId });
                return users.Single();
            }
        }
    }
}
