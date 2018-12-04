using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Queries
{
    public class GetLibrariesForUserHandler : IRequestHandler<GetLibrariesForUser, IEnumerable<LibraryDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetLibrariesForUserHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<LibraryDto>> Handle(GetLibrariesForUser request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT lib.id, lib.title, lib.description, lib.created_by, lib.created_at
                FROM plum.libraries lib
                INNER JOIN plum.memberships mem
                ON lib.id = mem.library_id
                WHERE mem.user_id = @UserId
                AND lib.deleted_at IS NULL";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return await cnn.QueryAsync<LibraryDto>(sql, new { request.UserId });
            }
        }
    }
}
