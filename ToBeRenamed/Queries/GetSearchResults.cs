using MediatR;
using ToBeRenamed.Models;

namespace ToBeRenamed.Queries
{
    public class GetSearchResults : IRequest<SearchResults>
    {
        public int UserId { get; }
        public string Query { get; }

        public GetSearchResults(int userId, string query)
        {
            UserId = userId;
            Query = query;
        }
    }
}
