using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Queries
{
    public class GetLibrariesCreatedByUserIdTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public GetLibrariesCreatedByUserIdTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task ItReturnsNone()
        {
            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Get all libraries created by that user
            var librariesRequest = new GetLibrariesCreatedByUserId(user.Id);
            var libraries = await _fixture.SendAsync(librariesRequest);

            // Check that none were returned
            Assert.Empty(libraries);
        }

        [Fact]
        [ResetDatabase]
        public async Task ItReturnsTheCorrectCount()
        {
            const string title = "My Fantastic Library";
            const string description = "A suitable description.";
            const int numOfLibraries = 12;

            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Send the same request to create a library many times (synchronously)
            var request = new CreateLibrary(user.Id, title, description);

            for (var i = 0; i < numOfLibraries; i++)
            {
                await _fixture.SendAsync(request);
            }

            // Get all libraries created by that user
            var librariesRequest = new GetLibrariesCreatedByUserId(user.Id);
            var libraries = await _fixture.SendAsync(librariesRequest);

            // Check that it returns the correct number of libraries
            Assert.True(libraries.Count() == numOfLibraries);
        }
    }
}
