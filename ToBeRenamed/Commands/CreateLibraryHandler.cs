using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class CreateLibraryHandler : IRequestHandler<CreateLibrary>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CreateLibraryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(CreateLibrary request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var title = request.Title;
            var description = request.Description;

            const string sql = @"
                WITH insertedLibrary AS (
                    INSERT INTO plum.libraries (title, description, created_by)
                    VALUES (@title, @description, @userId)
                    RETURNING id
                )
                INSERT INTO plum.memberships (user_id, library_id)
                VALUES (@userId, (SELECT id FROM insertedLibrary))";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, new { userId, title, description });
            }

            return Unit.Value;
        }
    }
}
