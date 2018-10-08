using System;
using System.IO;
using Dapper;
using Microsoft.Extensions.Configuration;
using ToBeRenamed.Dtos;
using Xunit;

namespace ToBeRenamed.Tests
{
    public class UnitTest1
    {
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

            using (var conn = connFactory.GetSqlConnection())
            {
                // Insert new user, then get the user id
                conn.Execute(insertUserSql);
//                return conn.Query<UserDto>(selectUserIdSql); // TODO - Replace UserDto with something in the ToBeRenamed.Tests namespace
            }
        }
    }
}
