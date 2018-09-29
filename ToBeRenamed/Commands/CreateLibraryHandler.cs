using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class CreateLibraryHandler : IRequestHandler<CreateLibrary>
    {
        private readonly SqlConnectionFactory _sqlConnectionFactory;

        public CreateLibraryHandler(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(CreateLibrary request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var title = request.Library.Title;
            var description = request.Library.Description;

            const string sql = @"
                INSERT INTO plum.libraries (title, description, created_by, created_at)
                VALUES (@title, @description, @userId, clock_timestamp())";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, new { userId, title, description });
            }

            return Unit.Value;
        }
    }
}
