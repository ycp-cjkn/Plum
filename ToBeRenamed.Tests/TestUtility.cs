using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Dtos;
using ToBeRenamed.Queries;

namespace ToBeRenamed.Tests
{
    public class TestUtility
    {
        private readonly DatabaseFixture _fixture;

        public TestUtility(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        public async Task<LibraryDto> CreateLibraryAsync()
        {
            const string title = "My Fantastic Library";
            const string description = "A suitable description.";

            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Create a library with that user
            var request = new CreateLibrary(user.Id, title, description);
            await _fixture.SendAsync(request);

            // Get all libraries created by that user
            var librariesRequest = new GetLibrariesCreatedByUserId(user.Id);
            return (await _fixture.SendAsync(librariesRequest)).Single();
        }
    }
}
