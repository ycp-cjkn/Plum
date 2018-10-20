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
        public async Task Should_ReturnTwoVideos_When_UserCreatesTwoVideos()
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
            
            // Create two videos for library
            const string vidTitle = "Video Title";
            const string videoUrl = "Video URL";
            const string videoUrl2 = "Video URL2";
            const string vidDescription = "Video description";
            
            var createVideosRequest1 = new AddVideo(user.Id, libraryId, vidTitle, videoUrl, vidDescription);
            var createVideosRequest2 = new AddVideo(user.Id, libraryId, vidTitle, videoUrl2, vidDescription);
            
            await _fixture.SendAsync(createVideosRequest1);
            await _fixture.SendAsync(createVideosRequest2);
            
            // Get videos for library
            var getVideosRequest = new GetVideosOfLibrary(libraryId);
            var videos = await _fixture.SendAsync(getVideosRequest);
            
            // Make sure correct number of videos returned
            var videoDtos = videos.ToList();
            Assert.Equal(2, videoDtos.Count);
            
            // Make sure the videos have the same content
            foreach (var video in videoDtos)
            {
                Assert.Equal(vidTitle, video.Title);
                Assert.Equal(vidDescription, video.Description);
            }

        }
    }
}
