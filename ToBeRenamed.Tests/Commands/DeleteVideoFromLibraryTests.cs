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

            var createVideosRequest1 = new CreateVideo(user.Id, libraryId, vidTitle, videoUrl, vidDescription);

            await _fixture.SendAsync(createVideosRequest1);

            // Get videos for library
            var getVideoIdRequest = new GetVideoById(libraryId);

            //delete video from library 
            //var deleteVideoRequest = new DeleteVideoFromLibrary();
        }
    }
}
