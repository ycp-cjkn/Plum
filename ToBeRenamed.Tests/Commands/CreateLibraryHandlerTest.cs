using Dapper;
using Respawn;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ToBeRenamed.Commands;
using ToBeRenamed.Dtos;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    [Collection("Database collection")]
    public class CreateLibraryHandlerTest
    {
        private readonly DatabaseFixture _fixture;

        public CreateLibraryHandlerTest(DatabaseFixture fixture)
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
        public void Should_InsertIntoDB_When_PropertiesAreValid()
        {
            _fixture.ResetDatabase(Checkpoint);

            var userId = _fixture.User.Id;
            const string title = "xUnitTitle";
            const string description = "xUnitDesc";

            var createLibrary = new CreateLibrary(userId, title, description);
            var createLibraryHandler = new CreateLibraryHandler(_fixture.ConnFactory);
            Task.Run(() => createLibraryHandler.Handle(createLibrary, new CancellationToken())).Wait();

            const string selectLibrarySql = @"
                SELECT id FROM plum.libraries
                WHERE 
                    title = @title 
                    AND description = @description
                    AND created_by = @userId";

            IEnumerable<LibraryDto> results;

            using (var cnn = _fixture.ConnFactory.GetSqlConnection())
            {
                results = cnn.Query<LibraryDto>(selectLibrarySql, new { userId, title, description });
            }

            Assert.Single(results);
        }
    }
}
