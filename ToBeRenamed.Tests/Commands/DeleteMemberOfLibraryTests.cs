using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class DeleteMemberOfLibraryTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public DeleteMemberOfLibraryTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        [ResetDatabase]
        public async Task ItDeletesLibraryMember()
        {
            const string title = "My Fantastic Library";
            const string description = "A suitable description.";

            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Create a library with that user
            var request = new CreateLibrary(user.Id, title, description);
            await _fixture.SendAsync(request);

            var deleteMemberRequest = new DeleteMemberOfLibrary(user.Id);
            await _fixture.SendAsync(deleteMemberRequest);

            Assert.True(user.Id == 0);
        }

    }
}
