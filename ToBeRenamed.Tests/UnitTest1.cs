using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Respawn;
using ToBeRenamed.Dtos;
using Xunit;

namespace ToBeRenamed.Tests
{
    [Collection("Database collection")]
    public class UnitTest1
    {
        DatabaseFixture fixture;

        public UnitTest1(DatabaseFixture fixture)
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
        public void Test1()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            var connFactory = new TestSqlConnectionFactory(configuration);
            
            // Insert new user
            const string insertUserSql = @"
                INSERT INTO plum.users (display_name, google_claim_nameidentifier)
                VALUES ('testUser', 1)";
            
            const string selectUserIdSql = @"
                SELECT id FROM plum.users WHERE display_name = 'testUser'";

            IEnumerable<UserDto> results;
            
            using (var conn = connFactory.GetSqlConnection())
            {
                // Insert new user, then get the user id
                conn.Execute(insertUserSql);
                results = conn.Query<UserDto>(selectUserIdSql); // TODO - Replace UserDto with something in the ToBeRenamed.Tests namespace
            }

            var userId = results.First().Id;
            string title = "xUnitTitle";
            var description = "xUnitDesc";
            
            const string insertLibrarySql = @"
                INSERT INTO plum.libraries (title, description, created_by)
                VALUES (@title, @description, @userId)";
            
            using (var cnn = connFactory.GetSqlConnection())
            {
                cnn.Execute(insertLibrarySql, new { userId, title, description });
            }
            
            using (var cnn = connFactory.GetSqlConnection())
            {
                // run synchronously
                Task.Run(() => cnn.Open()).Wait();
                Task.Run(() => checkpoint.Reset(cnn)).Wait();
            }
        }
    }
}
