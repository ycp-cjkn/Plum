using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Models;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class AddPrivilegesToRolesTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        private readonly TestUtility _utility;

        public AddPrivilegesToRolesTests(DatabaseFixture fixture)
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

            var privileges = new[] { Privilege.CanSubmitVideo, Privilege.CanRemoveOwnVideo };

            // Create request to add a privilege to a role
            var request = new AddPrivilegesToRole(roleId, new[] { privileges[0] });

            // Make sure we can send it twice without error (it shouldn't be duplicated)
            await _fixture.SendAsync(request);
            await _fixture.SendAsync(request);

            // Add another privilege
            var request2 = new AddPrivilegesToRole(roleId, new[] { privileges[1] });
            await _fixture.SendAsync(request2);

            // Get the roles for the library
            var roles = await _fixture.SendAsync(new GetRolesForLibrary(library.Id));

            // Check that the set of returned privileges is equal to our privileges
            Assert.True(roles.Single(r => r.Id == roleId).Privileges.SetEquals(privileges.ToHashSet()));
        }
    }
}
