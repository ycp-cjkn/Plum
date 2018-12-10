using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Queries
{
    public class GetAnnotationRepliesByVideoIdHandler : IRequestHandler<GetAnnotationRepliesByVideoId, IEnumerable<ReplyDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetAnnotationRepliesByVideoIdHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<ReplyDto>> Handle(GetAnnotationRepliesByVideoId request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT replies.id, annotation_id, text, replies.user_id
                FROM plum.replies
                INNER JOIN plum.annotations ON annotations.id = replies.annotation_id
                WHERE annotations.video_id = @videoId AND replies.deleted_at IS NULL";

            using (var conn = _sqlConnectionFactory.GetSqlConnection())
            {
                return await conn.QueryAsync<ReplyDto>(sql, new { request.VideoId });
            }
        }
    }
}