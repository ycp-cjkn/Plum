using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class UpdateVideoInfoTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public UpdateVideoInfoTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        [ResetDatabase]
        public async Task ItRunsAndEditsVideos()
        {
            //const int libraryId = 1;
            const string title = "My Fantastic Library";
            const string description = "A suitable description.";

            // Create a test user (don't think this is needed here)
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Create a library with that user
            var request = new CreateLibrary(user.Id, title, description);
            await _fixture.SendAsync(request);

            // Get all libraries created by that user
            var librariesRequest1 = new GetLibrariesForUser(user.Id);
            var libraries1 = (await _fixture.SendAsync(librariesRequest1)).ToArray();

            // Make sure there's only one library
            var libraryDtos = libraries1.ToList();
            Assert.Single(libraryDtos);

            // Get id of the single library
            var libraryId = libraryDtos.ToList().ElementAt(0).Id;

            // Create video for library
            const string vidTitle = "Video Title";
            const string newVidTitle = "My Fantastic New Video Title";
            const string videoUrl = "https://www.youtube.com/watch?v=-O5kNPlUV7w";
            const string vidDescription = "Video description";
            const string newVidDescription = "A (new) suitable video description.";

            var createVideosRequest1 = new CreateVideo(user.Id, libraryId, vidTitle, videoUrl, vidDescription);

            await _fixture.SendAsync(createVideosRequest1);

            // Get video for library
            var getVideoRequest = new GetVideosOfLibrary(libraryId);
            var video = await _fixture.SendAsync(getVideoRequest);

            // Edit video with that user
            var videoId = video.Single().Id;
            var updateVideoRequest = new UpdateVideoInfo(videoId, newVidTitle, newVidDescription);
            await _fixture.SendAsync(updateVideoRequest);

            // Get video created by that user
            var getVideoRequest1 = new GetVideosOfLibrary(libraryId);
            var video1 = await _fixture.SendAsync(getVideoRequest1);

            // Check that our video is the only video returned
            Assert.Single(video1);

            // Check that the fields match the fields we set
            Assert.Equal(newVidTitle, video1.Single().Title);
            Assert.Equal(newVidDescription, video1.Single().Description);
        }
    }
}