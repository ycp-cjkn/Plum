using System.Collections.Generic;
using System.Linq;
using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Queries
{
    public class CreateAnnotationHandler : IRequestHandler<CreateAnnotation, AnnotationDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CreateAnnotationHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<AnnotationDto> Handle(CreateAnnotation request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var comment = request.Comment;
            var timestamp = request.Timestamp;
            var videoId = request.VideoId;

            const string sql = @"
                WITH annotations AS (
                    INSERT INTO plum.annotations (user_id, comment, timestamp, video_id)
                    VALUES (@userId, @comment, @timestamp, @videoId)
                    RETURNING id, comment, timestamp, user_id, video_id
                )
                SELECT annotations.id, annotations.comment, annotations.timestamp, memberships.display_name FROM annotations
                INNER JOIN plum.memberships
                ON memberships.user_id = annotations.user_id
                INNER JOIN plum.videos
                ON memberships.library_id = videos.library_id AND videos.id = annotations.video_id";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                var annotation =  await cnn.QueryAsync<AnnotationDto>(sql, new { userId, comment, timestamp, videoId });
                return annotation.SingleOrDefault();
            }
        }
    }
}
