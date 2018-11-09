using System.Threading.Tasks;
using Dapper;
using MediatR;
using System.Threading;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class UpdateLibraryInfoHandler : IRequestHandler<UpdateLibraryInfo>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public UpdateLibraryInfoHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(UpdateLibraryInfo request, CancellationToken cancellationToken)
        {
            var libraryId = request.LibraryId;
            var newTitle = request.NewTitle;
            var newDescription = request.NewDescription;

            const string updateLibraryInfoSql = @"
                UPDATE plum.libraries
                SET title = @newTitle, description = @newDescription
                WHERE id = @libraryId"; 

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.QueryAsync(updateLibraryInfoSql, new {libraryId, newTitle, newDescription});
            }

            return Unit.Value;
        }
    }
}