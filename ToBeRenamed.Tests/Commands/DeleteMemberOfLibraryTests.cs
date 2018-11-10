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

            // Create a test user
            var userRequest = new CreateUserWithoutAuth("Alice");
            var user = await _fixture.SendAsync(userRequest);

            // Create a library with that user
            var request = new CreateLibrary(user.Id, title, description);
            await _fixture.SendAsync(request);

            // Get all libraries created by that user
            var librariesRequest = new GetLibrariesCreatedByUserId(user.Id);
            var libraries = (await _fixture.SendAsync(librariesRequest)).ToArray();

            //get single library from that user
            var library = libraries.Single();

            //get id for user
            var membershipRequest = new GetMembershipDto(user.Id, library.Id);
            var memberShipDto = await _fixture.SendAsync(membershipRequest);

            //delete user from library 
            var deleteMemberRequest = new DeleteMemberOfLibrary(memberShipDto.Id);
            await _fixture.SendAsync(deleteMemberRequest);

            //get members of library (should be 0)
            var memberRequest = new GetMembersOfLibrary(library.Id);
            var currentMembers = await _fixture.SendAsync(memberRequest);

            var member = currentMembers.Count();

            Assert.Equal(0, member);
        }

    }
}
