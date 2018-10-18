using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class CreateLibraryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public CreateLibraryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ItRunsAndCreatesLibrary()
        {
            _fixture.ResetDatabase();

            const string title = "My Fantastic Library";
            const string description = "A suitable description.";

            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Create a library with that user
            var request = new CreateLibrary(user.Id, title, description);
            await _fixture.SendAsync(request);

            // Get all libraries created by that user
            var librariesRequest = new GetLibrariesCreatedByUserId(user.Id);
            var libraries = (await _fixture.SendAsync(librariesRequest)).ToArray();

            // Check that our library is the only library returned
            Assert.Single(libraries);

            // Check that the fields match the feilds we set
            Assert.Equal(title, libraries.Single().Title);
            Assert.Equal(description, libraries.Single().Description);
        }
    }
}
