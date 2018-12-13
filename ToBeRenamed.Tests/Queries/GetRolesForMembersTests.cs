using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Queries
{
    public class GetRolesForMembersTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public GetRolesForMembersTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        [ResetDatabase]
        public async Task ShouldGetRolesForMembers()
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

            // Return member
            var getMembersRequest = new GetMembersOfLibrary(libraryId);
            var members = await _fixture.SendAsync(getMembersRequest);

            // Get that member's id
            var memberId = members.ToList().ElementAt(0).Id;

            // Put membership Id's into list
            var memberIds = new List<int>();
            memberIds.Add(memberId);

            // Retrieve roles from list
            var getRolesForMembersRequest = new GetRolesForMembers(memberIds);
            var memberRolesReturn = await _fixture.SendAsync(getRolesForMembersRequest);

            // Check to make sure roles returned from list
            Assert.Contains("Instructor", memberRolesReturn.Single().Value.Title);

        }
    }
}
