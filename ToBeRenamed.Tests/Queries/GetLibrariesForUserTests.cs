using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Queries
{
    public class GetLibrariesForUserTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public GetLibrariesForUserTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task Should_ReturnNone_When_UserBelongsToNoLibraries()
        {
            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Get all libraries created by that user
            var librariesRequest = new GetLibrariesForUser(user.Id);
            var libraries = await _fixture.SendAsync(librariesRequest);

            // Check that none were returned
            Assert.Empty(libraries);
        }

        // These tests currently fail.
        // Right now users don't get added to the memberships table when they create a library,
        // but once they do, these tests should pass.
        // TODO - Uncomment these tests when users get added to memberships table when they create a library
        [Fact]
        [ResetDatabase]
        public async Task Should_ReturnCorrectNumberOfLibraries_When_UserCreatesLibraries()
        {
            const string title = "My Fantastic Library";
            const string description = "A suitable description.";
            const int numOfLibraries = 3;

            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Send the same request to create a library many times (synchronously)
            var request = new CreateLibrary(user.Id, title, description);

            for (var i = 0; i < numOfLibraries; i++)
            {
                await _fixture.SendAsync(request);
            }

            // Get all libraries for that user
            var librariesRequest = new GetLibrariesForUser(user.Id);
            var libraries = await _fixture.SendAsync(librariesRequest);

            // Check that it returns the correct number of libraries
            Assert.True(libraries.Count() == numOfLibraries);
        }

        [Fact]
        [ResetDatabase]
        public async Task Should_ReturnCorrectNumberOfLibraries_When_MultipleUsersCreateLibraries()
        {
            const string title1 = "Library Title 1";
            const string description1 = "Library Description 1";
            const int numOfLibrariesFromUser1 = 2;
            
            const string title2 = "Library Title 2";
            const string description2 = "Library Description 2";
            const int numOfLibrariesFromUser2 = 3;
            
            // Create test users
            var userRequest1 = new CreateUserWithoutAuth("Alice");
            var user1 = await _fixture.SendAsync(userRequest1);
            
            var userRequest2 = new CreateUserWithoutAuth("Bob");
            var user2 = await _fixture.SendAsync(userRequest2);
            
            // Send the requests to create a library many times (synchronously) for user1
            var request1 = new CreateLibrary(user1.Id, title1, description1);

            for (var i = 0; i < numOfLibrariesFromUser1; i++)
            {
                await _fixture.SendAsync(request1);
            }
            
            // Send the requests to create a library many times (synchronously) for user2
            var request2 = new CreateLibrary(user2.Id, title2, description2);

            for (var i = 0; i < numOfLibrariesFromUser2; i++)
            {
                await _fixture.SendAsync(request2);
            }
            
            // Get all libraries for user1
            var librariesRequest1 = new GetLibrariesForUser(user1.Id);
            var libraries1 = await _fixture.SendAsync(librariesRequest1);
            
            // Get all libraries for user2
            var librariesRequest2 = new GetLibrariesForUser(user2.Id);
            var libraries2 = await _fixture.SendAsync(librariesRequest2);
            
            // Check that it returns the correct number of libraries for user1 and user2
            Assert.True(libraries1.Count() == numOfLibrariesFromUser1);
            Assert.True(libraries2.Count() == numOfLibrariesFromUser2);
        }
    }
}
