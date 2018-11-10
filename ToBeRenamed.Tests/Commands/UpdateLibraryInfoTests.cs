using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class UpdateLibraryInfoTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public UpdateLibraryInfoTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        [ResetDatabase]
        public async Task ItRunsAndEditsLibrary()
        {
            //const int libraryId = 1;
            const string title = "My Fantastic Library";
            const string description = "A suitable description.";
            const string newTitle = "My Fantastic New Library Title";
            const string newDescription = "A (new) suitable description.";

            // Create a test user (don't think this is needed here)
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Create a library with that user
            var request = new CreateLibrary(user.Id, title, description);
            await _fixture.SendAsync(request);

            // Get all libraries created by that user
            var librariesRequest1 = new GetLibrariesForUser(user.Id);
            var libraries1 = (await _fixture.SendAsync(librariesRequest1)).ToArray();

            // Edit a library with that user
            var libraryId = libraries1.Single().Id;
            var updateLibraryRequest = new UpdateLibraryInfo(libraryId, newTitle, newDescription);
            await _fixture.SendAsync(updateLibraryRequest);

            // Get all libraries created by that user
            var librariesRequest2 = new GetLibrariesForUser(user.Id);
            var libraries2 = (await _fixture.SendAsync(librariesRequest2)).ToArray();

            // Check that our library is the only library returned
            Assert.Single(libraries2);

            // Check that the fields match the fields we set
            Assert.Equal(newTitle, libraries2.Single().Title);
            Assert.Equal(newDescription, libraries2.Single().Description);
        }
    }
}
