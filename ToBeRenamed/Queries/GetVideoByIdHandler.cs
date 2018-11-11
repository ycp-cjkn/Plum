using Dapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Queries
{
    public class GetVideoByIdHandler : IRequestHandler<GetVideoById, VideoDto>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetVideoByIdHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<VideoDto> Handle(GetVideoById request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT videos.id, videos.title, videos.description, videos.library_id, video_urls.url
                FROM plum.videos
                INNER JOIN plum.video_urls
                ON videos.video_url_id = video_urls.id
                WHERE videos.id = @Id";

            using (var conn = _sqlConnectionFactory.GetSqlConnection())
            {
                return (await conn.QueryAsync<VideoDto>(sql, new { request.Id })).Single();
            }
        }
    }
}