using Dapper;
using Respawn;
using Xunit;

namespace ToBeRenamed.Tests
{
    [Collection("Database collection")]
    public class UnitTest1
    {
        private readonly DatabaseFixture _fixture;

        public UnitTest1(DatabaseFixture fixture)
        {
            _fixture = fixture;
        }

        private static readonly Checkpoint Checkpoint = new Checkpoint
        {
            SchemasToInclude = new[]
            {
                "plum",
                "public"
            },
            TablesToIgnore = new[]
            {
                "users"
            },
            DbAdapter = DbAdapter.Postgres
        };

        [Fact]
        public void Test1()
        {
            _fixture.ResetDatabase(Checkpoint);

            var userId = _fixture.User.Id;
            const string title = "xUnitTitle";
            const string description = "xUnitDesc";

            const string insertLibrarySql = @"
                INSERT INTO plum.libraries (title, description, created_by)
                VALUES (@title, @description, @userId)";

            using (var cnn = _fixture.ConnFactory.GetSqlConnection())
            {
                cnn.Execute(insertLibrarySql, new { userId, title, description });
            }
        }
    }
}
