using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class DeleteVideoFromLibraryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public DeleteVideoFromLibraryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task ItDeletesVideoFromLibrary()
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

            // Create video for library
            const string vidTitle = "Video Title";
            const string videoUrl = "https://www.youtube.com/watch?v=-O5kNPlUV7w";
            const string vidDescription = "Video description";

            var createVideosRequest = new CreateVideo(user.Id, libraryId, vidTitle, videoUrl, vidDescription);
            await _fixture.SendAsync(createVideosRequest);

            // Get videos just created by user
            var getVideosRequest = new GetVideosOfLibrary(libraryId);
            var videos = await _fixture.SendAsync(getVideosRequest);

            // Make sure there's only one video
            var videoDtos = videos.ToList();
            Assert.Single(videoDtos);

            var videoId = videoDtos.ToList().ElementAt(0).Id;

            //delete video from library 
            var deleteVideoRequest = new DeleteVideoFromLibrary(videoId);
            await _fixture.SendAsync(deleteVideoRequest);

            // Check for any remaining videos (should be 0)
            var getVideosRequest1 = new GetVideosOfLibrary(libraryId);
            var videosLeft = await _fixture.SendAsync(getVideosRequest1);

            var videoCount = videosLeft.Count();

            Assert.Equal(0, videoCount);
        }
    }
}
