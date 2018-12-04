using System.Linq;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Queries;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class UpdateDisplayNameTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public UpdateDisplayNameTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }
        [Fact]
        [ResetDatabase]
        public async Task It_Updates_A_DisplayName()
        {
            //create user named Alice
            var request = new CreateUserWithoutAuth("Alice");
            var result = await _fixture.SendAsync(request);

            //change name Alice to Bob
            var request1 = new UpdateDisplayName(result.Id, "Bob");
            await _fixture.SendAsync(request1);

            //retrieve user info through same id 
            var request2 = new GetUserByUserId(result.Id);
            var result1 = await _fixture.SendAsync(request2);

            //Bob should be new name 
            Assert.Equal("Bob", result1.DisplayName);
        }
    }
}
