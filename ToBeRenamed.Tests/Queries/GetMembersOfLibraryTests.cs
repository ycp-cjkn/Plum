using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Queries
{
    public class GetMembersOfLibraryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public GetMembersOfLibraryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task Should_ReturnOneMember_When_UserCreatesLibrary()
        {
            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            const string title = "My Fantastic Library";
            const string description = "A suitable description.";
            
            // User creates library
            var createLibraryRequest = new CreateLibrary(user.Id, title, description);
            await _fixture.SendAsync(createLibraryRequest);
            
            // Get all libraries created by that user
            var librariesRequest = new GetLibrariesCreatedByUserId(user.Id);
            var libraries = await _fixture.SendAsync(librariesRequest);

            // Check that only one library was returned
            Assert.Single(libraries);

            // Check that the created library only has one member, the creator
            var libraryId = libraries.ToList().ElementAt(0).Id;
            
            //Return that member
            var getMembersRequest = new GetMembersOfLibrary(libraryId);
            var members = await _fixture.SendAsync(getMembersRequest);

            Assert.Single(members);

            //Check that member by id
            var memberUserId = members.ToList().ElementAt(0).UserId;
            Assert.Equal(user.Id, memberUserId);
        }
    }
}
