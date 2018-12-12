using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class CreateAnnotationReplyHandler : IRequestHandler<CreateAnnotationReply, ReplyDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CreateAnnotationReplyHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<ReplyDto> Handle(CreateAnnotationReply request, CancellationToken cancellationToken)
        {
            const string sql = @"
                WITH REP AS (
                    INSERT INTO plum.replies (user_id, text, annotation_id)
                    VALUES (@UserId, @Text, @AnnotationId)
                    RETURNING id, text, user_id, annotation_id
                )
                SELECT
                    REP.id,
                    REP.text,
                    REP.annotation_id,
                    REP.user_id,
                    (CASE
                        WHEN mem.display_name = ''
                        THEN USR.display_name
                        ELSE mem.display_name END) AS display_name
                FROM REP
                INNER JOIN plum.annotations ANN
                ON ANN.id = @AnnotationId
                INNER JOIN plum.videos VID
                ON VID.id = ANN.video_id
                INNER JOIN plum.memberships MEM
                ON MEM.user_id = REP.user_id AND VID.library_id = MEM.library_id
                INNER JOIN plum.users USR
                ON REP.user_id = USR.id";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return (await cnn.QueryAsync<ReplyDto>(sql, request)).SingleOrDefault();
            }
        }
    }
}
