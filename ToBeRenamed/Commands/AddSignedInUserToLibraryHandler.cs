using Dapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Extensions;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Commands
{
    public class AddSignedInUserToLibraryHandler : IRequestHandler<AddSignedInUserToLibrary>
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public AddSignedInUserToLibraryHandler(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(AddSignedInUserToLibrary request, CancellationToken cancellationToken)
        {
            const string sql = @"
                INSERT INTO plum.memberships(user_id, library_id, role_id)
                SELECT id, @LibraryId, @RoleId FROM plum.users 
                WHERE google_claim_nameidentifier = @NameIdentifier
                ON CONFLICT DO NOTHING";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, new
                {
                    NameIdentifier = request.User.GetNameIdentifier(),
                    request.LibraryId,
                    request.RoleId
                });
            }

            return Unit.Value;
        }
    }
}
