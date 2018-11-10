using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Models;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class ReplacePrivilegesOfRolesTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly TestUtility _utility;

        public ReplacePrivilegesOfRolesTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _utility = new TestUtility(_fixture);
        }

        [Fact]
        [ResetDatabase]
        public async Task Test()
        {
            var library = await _utility.CreateLibraryAsync();

            // Create a new role
            var roleId = await _fixture.SendAsync(new CreateRole("Student", library.Id));

            var allPriviliges = Privilege.All().ToArray();
            var oldPriviliges = new[] { allPriviliges[0] };
            var newPriviliges = new[] { allPriviliges[1], allPriviliges[2] };

            // Add a privilege
            await _fixture.SendAsync(new AddPrivilegesToRole(roleId, oldPriviliges));

            // Replace privilege
            var request = new ReplacePrivilegesOfRole(roleId, newPriviliges);
            await _fixture.SendAsync(request);

            // Get the roles for the library
            var roles = await _fixture.SendAsync(new GetRolesForLibrary(library.Id));

            // Check that the set of returned privileges is equal to our privileges
            Assert.True(roles.Single(r => r.Id == roleId).Privileges.SetEquals(newPriviliges));
        }
    }
}
