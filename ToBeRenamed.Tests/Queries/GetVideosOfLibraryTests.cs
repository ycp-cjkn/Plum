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

        [Fact]
        [ResetDatabase]
        public async Task Should_ReturnNone_When_LibraryHasNoVideos()
        {
            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            const string title = "My Fantastic Library";
            const string description = "A suitable description.";
            
            // User creates library
            var createLibraryRequest = new CreateLibrary(user.Id, title, description);
            await _fixture.SendAsync(createLibraryRequest);
            
            // Get libraries just created by user
            var getLibrariesRequest = new GetLibrariesForUser(user.Id);
            var libraries = await _fixture.SendAsync(getLibrariesRequest);
            
            // Make sure there's only one library
            var libraryDtos = libraries.ToList();
            Assert.Single(libraryDtos);
            
            // Get id of the single library
            var libraryId = libraryDtos.ToList().ElementAt(0).Id;
            
            // Get videos for library
            var getVideosRequest = new GetVideosOfLibrary(libraryId);
            var videos = await _fixture.SendAsync(getVideosRequest);
            
            // Make sure correct number of videos returned
            var videoDtos = videos.ToList();
            Assert.Empty(videoDtos);
        }
    }
}
