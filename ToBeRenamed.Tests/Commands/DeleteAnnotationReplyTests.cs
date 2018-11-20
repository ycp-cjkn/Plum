using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class DeleteAnnotationReplyTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public DeleteAnnotationReplyTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task Should_ReturnZero_When_UserDeletesReply()
        {
            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Bebe");
            var user = await _fixture.SendAsync(userRequest);

            const string title = "My Cat Thumper";
            const string description = "Thumper is 12 years old";
            
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
            
            // Create 1 annotation
            var comment1 = "This is a comment!";
            var timestamp1 = 2.05;
            
            var annotationRequest1 = new CreateAnnotation(user.Id, comment1, video.Id, timestamp1);

            await _fixture.SendAsync(annotationRequest1);
            
            // get annotations
            var getAnnotationsRequest = new GetAnnotationsByVideoId(video.Id);
            var results = await _fixture.SendAsync(getAnnotationsRequest);
            var annotation = results.ToList().ElementAt(0);

            // create reply
            var replyText1 = "Such a great annotation";
            var createReplyRequest1 = new CreateAnnotationReply(user.Id, annotation.Id, replyText1);
            await _fixture.SendAsync(createReplyRequest1);
            
            // Get reply
            var getReplyRequest = new GetAnnotationRepliesByVideoId(video.Id);
            var replyResults1 = await _fixture.SendAsync(getReplyRequest);
            var reply = replyResults1.Single();
           
            // delete reply
            var deleteReplyRequest = new DeleteAnnotationReply(user.Id, reply.Id);
            await _fixture.SendAsync(deleteReplyRequest);

            // get replies
            var getRepliesRequest = new GetAnnotationRepliesByVideoId(video.Id);
            var replyResults = await _fixture.SendAsync(getRepliesRequest);
            var replies = replyResults.ToList();
            
            // Check that no replies are in database
            Assert.Empty(replies);
        }
    }
}
