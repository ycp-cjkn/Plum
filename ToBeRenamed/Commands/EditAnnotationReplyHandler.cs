using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class EditAnnotationReplyHandler : IRequestHandler<EditAnnotationReply, ReplyDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public EditAnnotationReplyHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<ReplyDto> Handle(EditAnnotationReply request, CancellationToken cancellationToken)
        {
            const string sql = @"
                WITH REP AS (
                      UPDATE plum.replies
                      SET text = @NewText
                      WHERE user_id = @UserId AND annotation_id = @AnnotationId 
                      RETURNING id, text, user_id, annotation_id
                )
            SELECT REP.id, REP.text, REP.annotation_id, MEM.display_name FROM REP
            INNER JOIN plum.memberships MEM
            ON MEM.user_id = REP.user_id";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return (await cnn.QueryAsync<ReplyDto>(sql, request)).SingleOrDefault();
            }
        }
    }
}
