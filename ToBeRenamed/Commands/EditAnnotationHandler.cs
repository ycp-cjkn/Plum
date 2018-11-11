using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class EditAnnotationHandler : IRequestHandler<EditAnnotation>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public EditAnnotationHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(EditAnnotation request, CancellationToken cancellationToken)
        {
            var comment = request.Comment;
            var annotationId = request.AnnotationId;
            var userId = request.UserId;
            
            const string sql = @"
                UPDATE plum.annotations
                SET comment = @comment, modified_at = NOW()
                WHERE 
                    user_id = @userId
                    AND id = @annotationId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, new {comment, annotationId, userId});
            }

            return Unit.Value;
        }
    }
}
