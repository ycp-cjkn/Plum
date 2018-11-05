using System.Threading.Tasks;
using Dapper;
using MediatR;
using System.Threading;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class UpdateLibraryDescHandler : IRequestHandler<UpdateLibraryDesc>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public UpdateLibraryDescHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(UpdateLibraryDesc request, CancellationToken cancellationToken)
        {
            var newDesc = request.NewDesc;
            var membershipId = request.MembershipId;

            const string updateLibraryDescSql = @"
                UPDATE plum.libraries
                SET description = @newDesc
                WHERE id = @membershipId";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.QueryAsync(updateLibraryDescSql, new { newDesc, membershipId });
            }

            return Unit.Value;
        }
    }
}