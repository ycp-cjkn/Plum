using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Respawn;
using ToBeRenamed.Commands;
using ToBeRenamed.Dtos;
using Xunit;

namespace ToBeRenamed.Tests.Commands
{
    [Collection("Database collection")]
    public class CreateLibraryHandlerTest
    {
        DatabaseFixture fixture;

        public CreateLibraryHandlerTest(DatabaseFixture fixture)
        {
            this.fixture = fixture;
        }
        
        private static Checkpoint checkpoint = new Checkpoint
        {
            SchemasToInclude = new[]
            {
                "plum",
                "public"
            },TablesToIgnore = new[]
            {
                "users"
            },
            DbAdapter = DbAdapter.Postgres
            
        };
            
        [Fact]
        public void Should_InsertIntoDB_When_PropertiesAreValid()
        {
            fixture.resetDatabase(checkpoint);
            
            var userId = fixture.User.Id;
            string title = "xUnitTitle";
            var description = "xUnitDesc";
            
            CreateLibrary createLibrary = new CreateLibrary(userId, title, description);
            CreateLibraryHandler createLibraryHandler = new CreateLibraryHandler(fixture.ConnFactory);
            Task.Run(() => createLibraryHandler.Handle(createLibrary, new CancellationToken())).Wait();
            
            const string selectLibrarySql = @"
                SELECT id FROM plum.libraries
                WHERE 
                    title = @title 
                    AND description = @description
                    AND created_by = @userId";

            IEnumerable<LibraryDto> results;
            
            using (var cnn = fixture.ConnFactory.GetSqlConnection())
            {
                results = cnn.Query<LibraryDto>(selectLibrarySql, new { userId, title, description });
            }
            
            Assert.Single(results);
        }
    }
}
