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
    public class GetSignedInUserDtoHandler : IRequestHandler<GetSignedInUserDto, UserDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetSignedInUserDtoHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<UserDto> Handle(GetSignedInUserDto request, CancellationToken cancellationToken)
        {
            var nameIdentifier = request.User.GetNameIdentifier();

            const string sql = @"
                SELECT id, display_name, google_claim_nameidentifier, created_at
                FROM plum.users
                WHERE google_claim_nameidentifier = @nameIdentifier";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                var users = await cnn.QueryAsync<UserDto>(sql, new { nameIdentifier });
                return users.Single();
            }
        }
    }
}
