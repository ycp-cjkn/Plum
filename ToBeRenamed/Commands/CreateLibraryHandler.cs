using Dapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Dtos;
using ToBeRenamed.Factories;
using ToBeRenamed.Models;

namespace ToBeRenamed.Commands
{
    public class CreateLibraryHandler : IRequestHandler<CreateLibrary>
    {
        private readonly IMediator _mediator;
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public CreateLibraryHandler(IMediator mediator, ISqlConnectionFactory sqlConnectionFactory)
        {
            _mediator = mediator;
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public async Task<Unit> Handle(CreateLibrary request, CancellationToken cancellationToken)
        {
            // Create library
            var library = await CreateLibrary(request.Title, request.Description, request.UserId);

            // Create role(s)
            var roleId = await _mediator.Send(new CreateRole("Instructor", library.Id), cancellationToken);

            // Add all privileges to new role
            await _mediator.Send(new AddPrivilegesToRole(roleId, Privilege.All()), cancellationToken);

            // Add a membership for the user creating the library
            await CreateMembership(library.Id, library.CreatedBy, roleId);

            return Unit.Value;
        }

        // This may become it's own command
        private async Task<LibraryDto> CreateLibrary(string title, string description, int userId)
        {
            const string sql = @"
                INSERT INTO plum.libraries (title, description, created_by)
                VALUES (@title, @description, @userId)
                RETURNING id, title, description, created_by, created_at";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                return (await cnn.QueryAsync<LibraryDto>(sql, new { title, description, userId })).Single();
            }
        }

        // This may become it's own command
        private async Task CreateMembership(int libraryId, int userId, int roleId)
        {
            const string sql = @"
                INSERT INTO plum.memberships (library_id, user_id, role_id)
                VALUES (@libraryId, @userId, @roleId)";

            using (var cnn = _sqlConnectionFactory.GetSqlConnection())
            {
                await cnn.ExecuteAsync(sql, new { libraryId, userId, roleId });
            }
        }
    }
}
