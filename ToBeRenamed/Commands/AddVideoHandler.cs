using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class AddVideoHandler : IRequestHandler<AddVideo>
    {
        private readonly SqlConnectionFactory _sqlConnectionFactory;

        public AddVideoHandler(SqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(AddVideo request, CancellationToken cancellationToken)
        {
            var userId = request.UserId;
            var title = request.Title;
            var link = request.Link;
            var description = request.Description;

            const string sql = @""; //need to find query to add video to two databases 

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, new { userId, title, link, description });
            }

            return Unit.Value;
        }
    }
}
