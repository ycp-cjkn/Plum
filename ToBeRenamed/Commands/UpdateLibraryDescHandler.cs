using System.Threading.Tasks;
using Dapper;
using MediatR;
using System.Threading;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class UpdateLibraryDescHandler : IRequestHandler<UpdateLibraryDesc>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public UpdateLibraryDescHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(UpdateLibraryDesc request, CancellationToken cancellationToken)
        {
            var newDesc = request.NewDesc;
            var libraryId = request.LibraryId;

            const string updateLibraryDescSql = @"
                UPDATE plum.libraries
                SET description = @newDesc
                WHERE id = @libraryId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.QueryAsync(updateLibraryDescSql, new { newDesc, libraryId });
            }

            return Unit.Value;
        }
    }
}