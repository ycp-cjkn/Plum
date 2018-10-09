using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Respawn;
using ToBeRenamed.Dtos;

namespace ToBeRenamed.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            // ... initialize data in the test database ...
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            ConnFactory = new TestSqlConnectionFactory(configuration);
            User = insertUser();
        }
        
        public UserDto User;

        public TestSqlConnectionFactory ConnFactory;

        public void Dispose()
        {
            // ... clean up test data from the database ...
            Checkpoint checkpoint = new Checkpoint
            {
                SchemasToInclude = new[]
                {
                    "plum",
                    "public"
                },
                DbAdapter = DbAdapter.Postgres
            };
            
            // Remove initial user
            using (var cnn = ConnFactory.GetSqlConnection())
            {
                // run synchronously
                Task.Run(() => cnn.Open()).Wait();
                Task.Run(() => checkpoint.Reset(cnn)).Wait();
            }
        }

        private UserDto insertUser()
        {
            // Insert new user
            const string insertUserSql = @"
                INSERT INTO plum.users (display_name, google_claim_nameidentifier)
                VALUES ('testUser', 1)";
            
            const string selectUserIdSql = @"
                SELECT id FROM plum.users WHERE display_name = 'testUser'";

            IEnumerable<UserDto> results;
            
            using (var conn = ConnFactory.GetSqlConnection())
            {
                // Insert new user, then get the user id
                conn.Execute(insertUserSql);
                results = conn.Query<UserDto>(selectUserIdSql); // TODO - Replace UserDto with something in the ToBeRenamed.Tests namespace
            }

            return results.First();
        }
    }

}