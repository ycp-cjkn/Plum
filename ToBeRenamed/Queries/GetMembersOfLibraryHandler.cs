using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Queries
{
    public class GetMembersOfLibraryHandler : IRequestHandler<GetMembersOfLibrary, IEnumerable<MemberDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetMembersOfLibraryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<MemberDto>> Handle(GetMembersOfLibrary request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
	                mem.id,
	                usr.id AS user_id,
	                lib.id AS library_id,
	                mem.display_name,
                    (CASE WHEN (mem.display_name = '') THEN usr.display_name ELSE mem.display_name END),
	                usr.id = lib.created_by AS is_creator
                FROM plum.users usr
                INNER JOIN plum.memberships mem
                ON usr.id = mem.user_id
                INNER JOIN plum.libraries lib
                ON lib.id = mem.library_id
                WHERE mem.library_id = @LibraryId
                ORDER BY mem.created_at";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return await cnn.QueryAsync<MemberDto>(sql, new { request.LibraryId });
            }
        }
    }
}