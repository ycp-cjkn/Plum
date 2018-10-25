using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Queries
{
    public class GetVideosOfLibraryHandler : IRequestHandler<GetVideosOfLibrary, IEnumerable<VideoDto>>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetVideosOfLibraryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<IEnumerable<VideoDto>> Handle(GetVideosOfLibrary request, CancellationToken cancellationToken)
        {
            const string sql = @"
                SELECT
                    videos.title,
                    videos.description,
                    videos.id
                FROM plum.videos
                INNER JOIN plum.memberships mem
                ON videos.library_id = mem.library_id
                WHERE videos.library_id = @LibraryId
                ORDER BY videos.created_at DESC";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return await cnn.QueryAsync<VideoDto>(sql, new { request.LibraryId });
            }
        }
    }
}