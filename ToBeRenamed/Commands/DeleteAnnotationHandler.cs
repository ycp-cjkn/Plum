using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class DeleteAnnotationHandler : IRequestHandler<DeleteAnnotation>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public DeleteAnnotationHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(DeleteAnnotation request, CancellationToken cancellationToken)
        {
            var annotationId = request.AnnotationId;
            var userId = request.UserId;
            
            const string sql = @"
                UPDATE plum.annotations
                SET deleted_at = NOW()
                WHERE 
                    user_id = @userId
                    AND id = @annotationId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, new {annotationId, userId});
            }

            return Unit.Value;
        }
    }
}
