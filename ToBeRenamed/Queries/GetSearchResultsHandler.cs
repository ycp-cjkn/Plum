using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;
using ToBeRenamed.Models;

namespace ToBeRenamed.Queries
{
    public class GetSearchResultsHandler : IRequestHandler<GetSearchResults, SearchResults>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public GetSearchResultsHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<SearchResults> Handle(GetSearchResults request, CancellationToken cancellationToken)
        {
            var librariesTask = SearchLibraries(request).ConfigureAwait(false);
            var videosTask = SearchVideos(request).ConfigureAwait(false);

            var libraries = (await librariesTask).ToList();
            var videos = (await videosTask).ToList();

            return new SearchResults(libraries, videos);
        }

        private async Task<IEnumerable<LibraryDto>> SearchLibraries(GetSearchResults request)
        {
            const string sql = @"
                SELECT lib.id, lib.title, lib.description, lib.created_by, lib.created_at
                FROM plum.libraries lib
                INNER JOIN plum.memberships mem
                ON lib.id = mem.library_id
                WHERE
	                to_tsvector(title || ' ' || description)
	                @@ plainto_tsquery(@Query)
	                AND mem.user_id = @UserId
                ORDER BY lib.created_at DESC";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return await cnn.QueryAsync<LibraryDto>(sql, request);
            }
        }

        private async Task<IEnumerable<VideoDto>> SearchVideos(GetSearchResults request)
        {
            const string sql = @"
                SELECT vid.id, vid.title, vid.description, vid.library_id
                FROM plum.videos vid
                INNER JOIN plum.memberships mem
                ON vid.library_id = mem.library_id
                WHERE
	                to_tsvector(title || ' ' || description)
	                @@ plainto_tsquery(@Query)
	                AND mem.user_id = @UserId
                ORDER BY vid.created_at DESC";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return await cnn.QueryAsync<VideoDto>(sql, request);
            }
        }
    }
}