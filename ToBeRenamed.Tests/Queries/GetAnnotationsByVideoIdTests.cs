using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Queries
{
    public class GetAnnotationsByVideoIdTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public GetAnnotationsByVideoIdTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task Should_ReturnNone_When_VideoHasNoAnnotations()
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
            
            // Get id of the single library
            var libraryDtos = libraries.ToList();
            var libraryId = libraryDtos.ElementAt(0).Id;
            
            // create video
            var vidTitle = "Video title";
            var vidLink = "Link goes here";
            var vidDescription = "Video description";
            
            var createVideoRequest = new CreateVideo(user.Id, libraryId, vidTitle, vidLink, vidDescription);
            await _fixture.SendAsync(createVideoRequest);
            
            // Get video
            var getVideosRequest = new GetVideosOfLibrary(libraryId);
            var videos = await _fixture.SendAsync(getVideosRequest);
            var video = videos.ToList().ElementAt(0);
            
            // Get annotations, and ensure none exist yet, since none have been created
            var getAnnotationsRequest = new GetAnnotationsByVideoId(video.Id);
            var annotations = await _fixture.SendAsync(getAnnotationsRequest);
            
            Assert.Empty(annotations);
        }
    }
}
