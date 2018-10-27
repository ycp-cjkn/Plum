using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
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
            const string getAnnotationsSql = @"
                SELECT
                    annotations.comment,
                    annotations.id,
                    annotations.timestamp,
                    users.display_name
                FROM plum.annotations
                INNER JOIN plum.users
                ON annotations.user_id = users.id
                WHERE annotations.video_id = @Id
                ORDER BY annotations.timestamp";

            using (var conn = _sqlConnectionFactory.GetSqlConnection())
            {
                return await conn.QueryAsync<AnnotationDto>(getAnnotationsSql, new {request.Id});
            }
        }
    }
}