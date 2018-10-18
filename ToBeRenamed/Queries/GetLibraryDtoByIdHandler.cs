using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Queries
{
    public class GetLibraryDtoByIdHandler : IRequestHandler<GetLibraryDtoById, LibraryDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetLibraryDtoByIdHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<LibraryDto> Handle(GetLibraryDtoById request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT id, title, description, created_by, created_at
                FROM plum.libraries
                WHERE id = @Id";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return (await cnn.QueryAsync<LibraryDto>(sql, new { request.Id })).Single();
            }
        }
    }
}