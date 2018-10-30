using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using MediatR;
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
            const string getVideoSql = @"
                SELECT title, description
                FROM plum.videos
                WHERE id = @Id";

            using (var conn = _sqlConnectionFactory.GetSqlConnection())
            {
                return (await conn.QueryAsync<VideoDto>(getVideoSql, new {request.Id})).Single();
            }
        }
    }
}