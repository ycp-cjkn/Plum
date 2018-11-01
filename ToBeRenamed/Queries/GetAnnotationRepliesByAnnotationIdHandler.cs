using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Queries
{
    public class GetAnnotationRepliesByAnnotationIdHandler : IRequestHandler<GetAnnotationRepliesByAnnotationId, IEnumerable<ReplyDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetAnnotationRepliesByAnnotationIdHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<ReplyDto>> Handle(GetAnnotationRepliesByAnnotationId request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
                    rep.id,
                    rep.text,
                    (CASE
                        WHEN mem.display_name = ''
                        THEN usr.display_name
                        ELSE mem.display_name END) AS display_name
                FROM plum.replies rep
                INNER JOIN plum.users usr
                ON rep.user_id = usr.id
                INNER JOIN plum.memberships mem
                ON rep.user_id = mem.user_id
                WHERE rep.annotation_id = @AnnotationId
                ORDER BY rep.created_at ASC, rep.modified_at ASC";

            using (var conn = _sqlConnectionFactory.GetSqlConnection())
            {
                return await conn.QueryAsync<ReplyDto>(sql, request);
            }
        }
    }
}