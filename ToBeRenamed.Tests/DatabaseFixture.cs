using Dapper;
using Microsoft.Extensions.Configuration;
using Respawn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<DatabaseFixture>()
                .Build();

            _fullResetCheckpoint = new Checkpoint
            {
                SchemasToInclude = new[]
                {
                    "plum",
                    "public"
                },
                DbAdapter = DbAdapter.Postgres
            };

            ConnFactory = new TestSqlConnectionFactory(configuration);
            ResetDatabase(_fullResetCheckpoint);
            User = InsertUser();
        }

        /// <summary>
        /// Used by all tests as the test user
        /// </summary>
        public UserDto User;

        /// <summary>
        /// Used by all tests to get database connections
        /// </summary>
        public TestSqlConnectionFactory ConnFactory;

        /// <summary>
        /// Checkpoint that will fully clear all database tables
        /// </summary>
        private readonly Checkpoint _fullResetCheckpoint;

        public void Dispose()
        {
            // ... clean up test data from the database ...
            // Remove initial user
            ResetDatabase(_fullResetCheckpoint);
        }

        /// <summary>
        /// Inserts a user into the database to be used with tests
        /// </summary>
        /// <returns>The user's data in a UserDto</returns>
        private UserDto InsertUser()
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

        /// <summary>
        /// Resets the database's tables using the parameters given by the checkpoint
        /// </summary>
        /// <param name="checkpoint">Contains info about how the database should reset</param>
        public void ResetDatabase(Checkpoint checkpoint)
        {
            using (var cnn = ConnFactory.GetSqlConnection())
            {
                // run synchronously
                Task.Run(() => cnn.Open()).Wait();
                Task.Run(() => checkpoint.Reset(cnn)).Wait();
            }
        }
    }
}