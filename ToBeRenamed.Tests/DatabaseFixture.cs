using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Respawn;

namespace ToBeRenamed.Tests
{
    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            // ... initialize data in the test database ...
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            var connFactory = new TestSqlConnectionFactory(configuration);
            
            Checkpoint checkpoint = new Checkpoint
            {
                SchemasToInclude = new[]
                {
                    "plum",
                    "public"
                },
                DbAdapter = DbAdapter.Postgres
            };
            
            using (var cnn = connFactory.GetSqlConnection())
            {
                // run synchronously
                Task.Run(() => cnn.Open()).Wait();
                Task.Run(() => checkpoint.Reset(cnn)).Wait();
            }
        }
    }

}