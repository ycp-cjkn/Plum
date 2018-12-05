using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class UpdateDisplayNameTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public UpdateDisplayNameTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        [ResetDatabase]
        public async Task It_Updates_A_DisplayName()
        {
            const string title = "My Fantastic Library";
            const string description = "A suitable description.";

            // Create a test user (don't think this is needed here)
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Create a library with that user
            var request = new CreateLibrary(user.Id, title, description);
            await _fixture.SendAsync(request);

            // Get library created by that user
            var librariesRequest = new GetLibrariesForUser(user.Id);
            var library = (await _fixture.SendAsync(librariesRequest)).ToArray();
            var libraryId = library.Single().Id;

            //Return that member
            var getMemberRequest = new GetMembersOfLibrary(libraryId);
            var member = await _fixture.SendAsync(getMemberRequest);

            //Update name from Alice to Bob 
            var updateDisplayNameRequest = new UpdateDisplayName(member.Single().Id, "Bob");
            await _fixture.SendAsync(updateDisplayNameRequest);

            //Return same member with changed display name
            var getMemberRequest1 = new GetMembersOfLibrary(member.Single().Id);
            var member1 = await _fixture.SendAsync(getMemberRequest1);

            //Bob should be new name 
            Assert.Equal("Bob", member1.Single().DisplayName);
        }
    }
}
