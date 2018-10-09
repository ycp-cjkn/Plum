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
    public class UnitTest2
    {
        DatabaseFixture fixture;

        public UnitTest2(DatabaseFixture fixture)
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
        public void Test2()
        {
            using (var cnn = fixture.ConnFactory.GetSqlConnection())
            {
                // run synchronously
                Task.Run(() => cnn.Open()).Wait();
                Task.Run(() => checkpoint.Reset(cnn)).Wait();
            }
            
            var userId = fixture.User.Id;
            string title = "xUnitTitle";
            var description = "xUnitDesc";
            
            const string insertLibrarySql = @"
                INSERT INTO plum.libraries (title, description, created_by)
                VALUES (@title, @description, @userId)";
            
            using (var cnn = fixture.ConnFactory.GetSqlConnection())
            {
                cnn.Execute(insertLibrarySql, new { userId, title, description });
            }
        }
    }
}
