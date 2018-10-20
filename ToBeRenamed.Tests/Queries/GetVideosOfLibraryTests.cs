using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Queries
{
    public class GetVideosOfLibraryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public GetVideosOfLibraryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

//        [Fact]
//        [ResetDatabase]
//        public async Task Should_ReturnOneMember_When_UserCreatesLibrary()
//        {
//            // Create a test user
//            var userRequest = new CreateUserWithoutAuth("Alice");
//            var user = await _fixture.SendAsync(userRequest);
//
//            const string title = "My Fantastic Library";
//            const string description = "A suitable description.";
//            
//            // User creates library
//            var createLibraryRequest = new CreateLibrary(user.Id, title, description);
//            await _fixture.SendAsync(createLibraryRequest);
//            
//            // TODO - User creates video
//        }
    }
}
