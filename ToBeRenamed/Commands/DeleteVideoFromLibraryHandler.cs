using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class DeleteVideoFromLibraryHandler : IRequestHandler<DeleteVideoFromLibrary>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public DeleteVideoFromLibraryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(DeleteVideoFromLibrary request, CancellationToken cancellationToken)
        {
            var deletedAt = request.DeletedAt;
            var videoId = request.VideoId;

            const string sql = @"
                UPDATE plum.videos
                SET deleted_at = @deletedAt
                WHERE video_id = @videoId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.QueryAsync(sql, new { deletedAt, videoId });
            }

            return Unit.Value;
        }
    }
}
