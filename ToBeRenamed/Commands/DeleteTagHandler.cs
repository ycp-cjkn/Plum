using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class DeleteTagHandler : IRequestHandler<DeleteTag>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public DeleteTagHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(DeleteTag request, CancellationToken cancellationToken)
        {
            var tagId = request.Id;
            var text = request.Tag;

            const string sql = @"
                UPDATE plum.tags 
                DELETE (id, text) WHERE id = @tagID 
                INNER JOIN plum.videotags VTAGS
                ON VTAGS.tag_id = @tagID";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, new { tagId, text });
            }

            return Unit.Value;
        }
    }
}
