using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class CreateVideoHandler : IRequestHandler<CreateVideo>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CreateVideoHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(CreateVideo request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var libraryId = request.LibraryId;
            var title = request.Title;
            var link = request.Link;
            var description = request.Description;

            // TODO - Remove this and add validation to make sure only valid youtube urls can get added
            var parsedUrl = link;
            if (link.Length == 43)
            {
                parsedUrl = link.Substring(32, 11);
            }
            
            const string sql = @"
            WITH videoURLS AS (
                INSERT INTO plum.video_urls (url)
                VALUES (@parsedUrl)
                RETURNING id, url
            )
            INSERT INTO plum.videos (title, description, video_url_id, library_id)
            SELECT @title, @description, id, @libraryId FROM videoURLS"; 

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, new { userId, libraryId, title, parsedUrl, description });
            }

            return Unit.Value;
        }
    }
}
