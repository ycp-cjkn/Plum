using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class CreateAnnotationReplyTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public CreateAnnotationReplyTests(DatabaseFixture fixture)
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
            var timestamp1 = 1.25;
            
            var annotationRequest1 = new CreateAnnotation(user.Id, comment1, video.Id, timestamp1);

            await _fixture.SendAsync(annotationRequest1);
            
            // get annotations
            var getAnnotationsRequest = new GetAnnotationsByVideoId(video.Id);
            var results = await _fixture.SendAsync(getAnnotationsRequest);
            var annotation = results.ToList().ElementAt(0);

            // create replies
            var replyText1 = "This is a reply";
            var replyText2 = "This is another reply";
            
            var createReplyRequest1 = new CreateAnnotationReply(user.Id, annotation.Id, replyText1);
            var createReplyRequest2 = new CreateAnnotationReply(user.Id, annotation.Id, replyText2);
            
            await _fixture.SendAsync(createReplyRequest1);
            await _fixture.SendAsync(createReplyRequest2);
            
            // get replies
            var getRepliesRequest = new GetAnnotationRepliesByAnnotationId(annotation.Id);
            var replyResults = await _fixture.SendAsync(getRepliesRequest);
            var replies = replyResults.ToList();

            var reply1 = replies.ElementAt(0);
            var reply2 = replies.ElementAt(1);
            
            // Check that both replies got created
            Assert.Equal(2, replies.Count);
            
            // Check that the properties of the replies are correct
            Assert.Equal(replyText1, reply1.Text);
            Assert.Equal(replyText2, reply2.Text);
        }
    }
}
