using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class DeleteAnnotationReplyHandler : IRequestHandler<DeleteAnnotationReply>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public DeleteAnnotationReplyHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(DeleteAnnotationReply request, CancellationToken cancellationToken)
        {
            const string sql = @"
                UPDATE plum.replies
                SET deleted_at = NOW()
                WHERE 
                    user_id = @UserId
                    AND id = @ReplyId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, request);
            }

            return Unit.Value;
        }
    }
}
