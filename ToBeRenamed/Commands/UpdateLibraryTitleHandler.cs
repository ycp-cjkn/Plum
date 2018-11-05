using System.Threading.Tasks;
using Dapper;
using MediatR;
using System.Threading;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class UpdateLibraryTitleHandler : IRequestHandler<UpdateLibraryTitle>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public UpdateLibraryTitleHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(UpdateLibraryTitle request, CancellationToken cancellationToken)
        {
            var newTitle = request.NewTitle;
            var libraryId = request.LibraryId;

            const string updateLibraryTitleSql = @"
                UPDATE plum.libraries
                SET title = @newTitle
                WHERE id = @libraryId"; 

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.QueryAsync(updateLibraryTitleSql, new { newTitle, libraryId });
            }

            return Unit.Value;
        }
    }
}