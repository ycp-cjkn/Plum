using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class UpdateRoleOfMemberTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public UpdateRoleOfMemberTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        [ResetDatabase]
        public async Task ItUpdatesRoleOfMember()
        {
            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            const string title = "My Fantastic Library";
            const string roleTitle = "Student";
            const string newRoleTitle = "Teaching Assistant";
            const string description = "A suitable description.";

            // User creates library
            var createLibraryRequest = new CreateLibrary(user.Id, title, description);
            await _fixture.SendAsync(createLibraryRequest);

            // Get libraries just created by user
            var getLibrariesRequest = new GetLibrariesForUser(user.Id);
            var libraries = await _fixture.SendAsync(getLibrariesRequest);

            // Make sure there's only one library
            var libraryDtos = libraries.ToList();
            Assert.Single(libraryDtos);

            // Get id of the single library
            var libraryId = libraryDtos.ToList().ElementAt(0).Id;

            // Return member
            var getMembersRequest = new GetMembersOfLibrary(libraryId);
            var members = await _fixture.SendAsync(getMembersRequest);

            // Get that member's id
            var memberUserId = members.ToList().ElementAt(0).UserId;

            // User creates role for that library
            var createRoleRequest = new CreateRole(roleTitle, libraryId);
            await _fixture.SendAsync(createRoleRequest);

            // Retrieve roles
            var getRoleRequest = new GetRolesForLibrary(libraryId);
            var role = await _fixture.SendAsync(getRoleRequest);

            // Make sure 1 role returned
            var roleDtos = role.ToList();

            // RoleId of role after default (our created role)
            var roleId = roleDtos.ToList().ElementAt(1).Id;

            // Update that role 
            var newRoleRequest = new UpdateRoleOfMember( memberUserId, roleId, newRoleTitle);
            await _fixture.SendAsync(getRoleRequest);

            // Retrieve that updated role
            var getNewRoleRequest = new GetRolesForLibrary(libraryId);
            var newRole = await _fixture.SendAsync(getNewRoleRequest);

            // Make sure the role was updated
            Assert.Contains(newRoleTitle, newRole.Select(r => r.Title));
        }
    }
}
