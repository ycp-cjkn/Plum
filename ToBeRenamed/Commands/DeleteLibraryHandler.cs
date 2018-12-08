using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class DeleteLibraryHandler : IRequestHandler<DeleteLibrary>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public DeleteLibraryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(DeleteLibrary request, CancellationToken cancellationToken)
        {
            var libraryId = request.LibraryId;

            const string sql = @"
                UPDATE plum.libraries
                SET deleted_at = NOW()
                WHERE id = @libraryId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.QueryAsync(sql, new { libraryId });
            }

            return Unit.Value;
        }
    }
}
