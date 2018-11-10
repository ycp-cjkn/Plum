using System;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;
using System.Linq;

namespace ToBeRenamed.Tests.Queries
{
    public class GetVideoByIdTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;
        
        public GetVideoByIdTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        
        [Fact]
        [ResetDatabase]
        public async Task Should_ReturnCorrectVideo_When_GivenValidId()
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
            const string videoTitle = "Video Title";
            const string videoUrl = "https://www.youtube.com/watch?v=-O5kNPlUV7w";
            const string videoDescription = "Video Description";
            
            var createVideosRequest1 = new CreateVideo(user.Id, libraryId, videoTitle, videoUrl, videoDescription);
            
            await _fixture.SendAsync(createVideosRequest1);
            
            // Get videos for library
            var getVideosRequest = new GetVideosOfLibrary(libraryId);
            var videos = await _fixture.SendAsync(getVideosRequest);
            
            // Make sure correct number of videos returned
            var videoDtos = videos.ToList();
            Assert.Single(videoDtos);
            
            // Get video id
            int videoId = videoDtos.ElementAt(0).Id;
            
            // Get the video using the id
            var getVideoRequest = new GetVideoById(videoId);
            var video = await _fixture.SendAsync(getVideoRequest);
            
            // Make sure the video's properties match the properties given on creation
            Assert.Equal(videoTitle, video.Title);
            Assert.Equal(videoDescription, video.Description);
        }
        
        [Fact]
        [ResetDatabase]
        public async Task Should_ThrowInvalidOperationException_When_GivenInvalidId()
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
            const string videoTitle = "Video Title";
            const string videoUrl = "https://www.youtube.com/watch?v=-O5kNPlUV7w";
            const string videoDescription = "Video Description";
            
            var createVideosRequest1 = new CreateVideo(user.Id, libraryId, videoTitle, videoUrl, videoDescription);
            
            await _fixture.SendAsync(createVideosRequest1);
            
            // Get videos for library
            var getVideosRequest = new GetVideosOfLibrary(libraryId);
            var videos = await _fixture.SendAsync(getVideosRequest);
            
            // Make sure correct number of videos returned
            var videoDtos = videos.ToList();
            Assert.Single(videoDtos);
            
            // Get video id
            int videoId = videoDtos.ElementAt(0).Id;
            
            // make videoId invalid
            videoId++; 
            
            // Create request with invalid id
            var getVideoRequest = new GetVideoById(videoId);
            
            // Make sure that an exception is thrown when video retrieval is attempted
            await Assert.ThrowsAsync<InvalidOperationException>(async() => await _fixture.SendAsync(getVideoRequest));
        }
    }
}
