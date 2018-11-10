using System.Linq;
using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class CreateRoleHandler : IRequestHandler<CreateRole, int>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CreateRoleHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<int> Handle(CreateRole request, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO plum.roles (title, library_id)
	            VALUES (@Title, @LibraryId)
                RETURNING id";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return (await cnn.QueryAsync<int>(sql, request)).Single();
            }
        }
    }
}
