using Dapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class CreateUserWithoutAuthHandler : IRequestHandler<CreateUserWithoutAuth, UserDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CreateUserWithoutAuthHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<UserDto> Handle(CreateUserWithoutAuth request, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO plum.users(display_name)
                VALUES (@DisplayName)
                RETURNING id, created_at, display_name";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return (await cnn.QueryAsync<UserDto>(sql, new { request.DisplayName })).Single();
            }
        }
    }
}
