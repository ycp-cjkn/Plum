using System.Threading.Tasks;
using ToBeRenamed.Commands;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    public class CreateUserWithoutAuthTests : IClassFixture<DatabaseFixture>
    {
        private readonly DatabaseFixture _fixture;

        public CreateUserWithoutAuthTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task ItRuns()
        {
            _fixture.ResetDatabase();

            var request = new CreateUserWithoutAuth("Alice");
            var result = await _fixture.SendAsync(request);

            Assert.NotNull(result);
            Assert.Equal("Alice", result.DisplayName);
            Assert.Null(result.GoogleClaimNameIdentifier);
        }
    }
}
