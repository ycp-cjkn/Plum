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
    public class GetAnnotationsByVideoIdHandler : IRequestHandler<GetAnnotationsByVideoId, IEnumerable<AnnotationDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetAnnotationsByVideoIdHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<AnnotationDto>> Handle(GetAnnotationsByVideoId request, CancellationToken cancellationToken)
        {
            const string annotationSql = @"
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

            const string repliesSql = @"
                SELECT annotation_id, text
                FROM plum.replies
                INNER JOIN plum.annotations ON annotations.id = replies.annotation_id
                WHERE annotations.video_id = @videoId
                ORDER BY replies.annotation_id ASC, replies.created_at DESC, replies.modified_at DESC";
            
            using (var conn = _sqlConnectionFactory.GetSqlConnection())
            {
                var annotationsResults = await conn.QueryAsync<AnnotationDto>(annotationSql, request);
                var repliesResults = await conn.QueryAsync<ReplyDto>(repliesSql, new { request.VideoId });

                var annotations = annotationsResults.ToList();
                var replies = repliesResults.ToList();

                for (int i = 0; i < annotations.Count; i++)
                {
                    annotations[i].Replies = new List<ReplyDto>();
                    for (int j = 0; j < replies.Count; j++)
                    {
                        if (annotations[i].Id == replies[j].AnnotationId)
                        {
                            annotations[i].Replies.Add(replies[j]);
                        }
                    }
                }

                return annotations;
            }
        }
    }
}