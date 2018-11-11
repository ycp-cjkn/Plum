using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class EditAnnotationsTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public EditAnnotationsTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task Should_ReturnCorrectData_When_UserEditsAnnotation()
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
            
            // Edit annotations
            var newComment1 = "New comment1!";
            var newComment2 = "New Comment2!";
            
            var editAnnotationRequest1 = new EditAnnotation(user.Id, newComment1, annotations.ElementAt(0).Id);
            var editAnnotationRequest2 = new EditAnnotation(user.Id, newComment2, annotations.ElementAt(1).Id);
            await _fixture.SendAsync(editAnnotationRequest1);
            await _fixture.SendAsync(editAnnotationRequest2);
            
            // Get edited annotations
            var getEditedAnnotationsRequest = new GetAnnotationsByVideoId(video.Id);
            var editedResults = await _fixture.SendAsync(getEditedAnnotationsRequest);
            var editedAnnotations = editedResults.ToList();
            
            // Check that there are two annotations
            Assert.Equal(2, editedAnnotations.Count);

            // Check that the comments of each annotation match the edited comments
            var annotation1 = editedAnnotations.ElementAt(0);
            var annotation2 = editedAnnotations.ElementAt(1);
            
            Assert.Equal(newComment1, annotation1.Comment);
            Assert.Equal(newComment2, annotation2.Comment);
        }
    }
}
