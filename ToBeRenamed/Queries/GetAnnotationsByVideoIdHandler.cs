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
                    ann.user_id,
                    (CASE
                        WHEN mem.display_name = ''
                        THEN usr.display_name
                        ELSE mem.display_name END)
                FROM plum.annotations ann
                INNER JOIN plum.videos vid
                ON ann.video_id = vid.id
                INNER JOIN plum.users usr
                ON ann.user_id = usr.id
                INNER JOIN plum.memberships mem
                ON ann.user_id = mem.user_id AND mem.library_id = vid.library_id
                WHERE 
                    ann.video_id = @VideoId
                    AND ann.deleted_at IS NULL
                ORDER BY ann.timestamp ASC";

            const string repliesSql = @"
                SELECT
                    rep.id,
                    rep.user_id,
                    rep.annotation_id,
                    rep.text,
                    (CASE
                        WHEN mem.display_name = ''
                        THEN usr.display_name
                        ELSE mem.display_name END)
                FROM plum.replies rep
                INNER JOIN plum.annotations ann
                ON ann.id = rep.annotation_id
                INNER JOIN plum.videos vid
                ON ann.video_id = vid.id
                INNER JOIN plum.users usr
                ON rep.user_id = usr.id
                INNER JOIN plum.memberships mem
                ON rep.user_id = mem.user_id AND mem.library_id = vid.library_id
                WHERE ann.video_id = @videoId AND rep.deleted_at IS NULL
                ORDER BY rep.annotation_id ASC, rep.created_at DESC, rep.modified_at DESC";
            
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