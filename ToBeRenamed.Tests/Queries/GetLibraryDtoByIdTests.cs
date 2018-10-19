using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Queries
{
    public class GetLibraryDtoByIdTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public GetLibraryDtoByIdTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task Should_ReturnCorrectLibraryData_When_LibraryIsCreated()
        {
            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Create a library for user
            const string title = "Library Title";
            const string description = "Library description";
            var createLibraryRequest = new CreateLibrary(user.Id, title, description);
            await _fixture.SendAsync(createLibraryRequest);
            
            // Get the library just created by the user
            var getLibrariesRequest = new GetLibrariesCreatedByUserId(user.Id);
            var libraries = await _fixture.SendAsync(getLibrariesRequest);
            
            // Make sure that only a single library was returned
            var libraryDtos = libraries.ToList();
            Assert.True(libraryDtos.Count() == 1);

            var library = libraryDtos.ElementAt(0);
            
            // Get library from database using id
            var getLibraryRequest = new GetLibraryDtoById(library.Id);
            var libraryDtoFromRequest = await _fixture.SendAsync(getLibraryRequest);
            
            // Make sure that retrieved library matches created one
            Assert.Equal(library.Id, libraryDtoFromRequest.Id);
            Assert.Equal(library.Description, libraryDtoFromRequest.Description);
            Assert.Equal(library.CreatedAt, libraryDtoFromRequest.CreatedAt);
            Assert.Equal(library.CreatedBy, libraryDtoFromRequest.CreatedBy);
        }
    }
}
