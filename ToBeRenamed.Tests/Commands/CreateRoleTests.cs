using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class CreateRoleTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public CreateRoleTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task ItCreatesRole()
        {
            const string title = "Student";
            const string libTitle = "My Fantastic Library";
            const string libDescription = "A suitable description.";

            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Create a library with that user
            var request = new CreateLibrary(user.Id, libTitle, libDescription);
            await _fixture.SendAsync(request);

            // Get all libraries created by that user
            var librariesRequest1 = new GetLibrariesForUser(user.Id);
            var libraries1 = (await _fixture.SendAsync(librariesRequest1)).ToArray();

            // Get libraryId of that library
            var libraryId = libraries1.Single().Id;

            //Create a role for that library
            var roleRequest = new CreateRole(title, libraryId);
            await _fixture.SendAsync(roleRequest);

            //Get roles for selected library
            var returnRoleRequest = new GetRolesForLibrary(libraryId);
            var result = await _fixture.SendAsync(returnRoleRequest);

            //make sure role matches our role title 
            Assert.Contains(title, result.Select(r => r.Title));

        }
    }
}
