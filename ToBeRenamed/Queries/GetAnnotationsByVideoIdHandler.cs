using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Queries
{
    public class GetAnnotationsByVideoIdHandler : IRequestHandler<GetAnnotationsByVideoId, IEnumerable<AnnotationDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetAnnotationsByVideoIdHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<AnnotationDto>> Handle(GetAnnotationsByVideoId request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
                    ann.id,
                    ann.comment,
                    ann.timestamp,
                    (CASE
                        WHEN mem.display_name = ''
                        THEN usr.display_name
                        ELSE mem.display_name END)
                FROM plum.annotations ann
                INNER JOIN plum.users usr
                ON ann.user_id = usr.id
                INNER JOIN plum.memberships mem
                ON ann.user_id = mem.user_id
                WHERE ann.video_id = @VideoId
                ORDER BY ann.timestamp DESC";

            using (var conn = _sqlConnectionFactory.GetSqlConnection())
            {
                return await conn.QueryAsync<AnnotationDto>(sql, request);
            }
        }
    }
}