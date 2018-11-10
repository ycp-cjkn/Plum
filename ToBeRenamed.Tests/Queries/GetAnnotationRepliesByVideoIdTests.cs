using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Queries
{
    public class GetAnnotationRepliesByVideoIdTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public GetAnnotationRepliesByVideoIdTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task Should_ReturnTwoReplies_When_UserCreatesTwoRepliesOnSameAnnotation()
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
            
            // create two videos
            var vidTitle = "Video title";
            var vidTitle2 = "Video title2";
            var vidLink = "Link goes here";
            var vidLink2 = "Link goes here2";
            var vidDescription = "Video description";
            var vidDescription2 = "Video description2";
            
            var createVideoRequest = new CreateVideo(user.Id, libraryId, vidTitle, vidLink, vidDescription);
            var createVideoRequest2 = new CreateVideo(user.Id, libraryId, vidTitle2, vidLink2, vidDescription2);
            await _fixture.SendAsync(createVideoRequest);
            await _fixture.SendAsync(createVideoRequest2);
            
            // Get video
            var getVideosRequest = new GetVideosOfLibrary(libraryId);
            var videosResults = await _fixture.SendAsync(getVideosRequest);
            var videos = videosResults.ToList();
            var video1 = videos.ToList().ElementAt(0);
            var video2 = videos.ToList().ElementAt(1);
            
            // Create an annotation on each video
            var comment1 = "This is a comment!";
            var timestamp1 = 1.25;
            var comment2 = "This is another comment!";
            var timestamp2 = 18.90;
            
            var annotationRequest1 = new CreateAnnotation(user.Id, comment1, video1.Id, timestamp1);
            var annotationRequest2 = new CreateAnnotation(user.Id, comment2, video2.Id, timestamp2);

            await _fixture.SendAsync(annotationRequest1);
            await _fixture.SendAsync(annotationRequest2);
            
            // get annotations
            var getAnnotationsRequest1 = new GetAnnotationsByVideoId(video1.Id);
            var getAnnotationsRequest2 = new GetAnnotationsByVideoId(video2.Id);
            
            var annotationResults1 = await _fixture.SendAsync(getAnnotationsRequest1);
            var annotationResults2 = await _fixture.SendAsync(getAnnotationsRequest2);
            
            var video1Annotation = annotationResults1.ToList().ElementAt(0);
            var video2Annotation = annotationResults2.ToList().ElementAt(0);

            // create replies
            var replyText1 = "This is a reply";
            var replyText2 = "This is another reply";
            
            var createReplyRequest1 = new CreateAnnotationReply(user.Id, video1Annotation.Id, replyText1);
            var createReplyRequest2 = new CreateAnnotationReply(user.Id, video2Annotation.Id, replyText2);
            
            await _fixture.SendAsync(createReplyRequest1);
            await _fixture.SendAsync(createReplyRequest2);
            
            // get replies
            var getRepliesRequest1 = new GetAnnotationRepliesByVideoId(video1.Id);
            var getRepliesRequest2 = new GetAnnotationRepliesByVideoId(video2.Id);
            
            var replyResults1 = await _fixture.SendAsync(getRepliesRequest1);
            var replyResults2 = await _fixture.SendAsync(getRepliesRequest2);
            
            var replies1 = replyResults1.ToList();
            var replies2 = replyResults2.ToList();

            var reply1 = replies1.ElementAt(0);
            var reply2 = replies2.ElementAt(0);
            
            // Check that the properties of the replies are correct
            Assert.Equal(replyText1, reply1.Text);
            Assert.Equal(replyText2, reply2.Text);
        }
    }
}
