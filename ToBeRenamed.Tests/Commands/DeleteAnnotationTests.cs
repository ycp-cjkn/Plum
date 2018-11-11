using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class DeleteAnnotationsTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public DeleteAnnotationsTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task Should_DeleteAnnotation_When_UserDeletesAnnotation()
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
            
            // Create two annotations
            var comment1 = "This is a comment!";
            var comment2 = "This is another comment!";
            var timestamp1 = 1.25;
            var timestamp2 = 18.45;
            
            var annotationRequest1 = new CreateAnnotation(user.Id, comment1, video.Id, timestamp1);
            var annotationRequest2 = new CreateAnnotation(user.Id, comment2, video.Id, timestamp2);

            await _fixture.SendAsync(annotationRequest1);
            await _fixture.SendAsync(annotationRequest2);
            
            // get annotations
            var getAnnotationsRequest = new GetAnnotationsByVideoId(video.Id);
            var results = await _fixture.SendAsync(getAnnotationsRequest);
            var annotations = results.ToList();
            
            // Check that two annotations were created
            Assert.Equal(2, annotations.Count);
            
            // Delete the first annotation in the list
            var annotation1 = annotations.ElementAt(0);
            
            var deleteAnnotationRequest = new DeleteAnnotation(user.Id, annotation1.Id);
            await _fixture.SendAsync(deleteAnnotationRequest);
            
            // Get annotations after deleting one of them and make sure there's only one now
            var getNewAnnotationsRequest = new GetAnnotationsByVideoId(video.Id);
            var afterDeleteResults = await _fixture.SendAsync(getNewAnnotationsRequest);
            var afterDeleteAnnotations = afterDeleteResults.ToList();
            
            // Check that there are two annotations
            Assert.Equal(1, afterDeleteAnnotations.Count);
            Assert.Equal(comment1, afterDeleteAnnotations.ElementAt(0).Comment);
        }
    }
}
