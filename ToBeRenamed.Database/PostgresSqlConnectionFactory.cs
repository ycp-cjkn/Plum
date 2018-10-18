using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data.Common;
using ToBeRenamed.Factories;

namespace ToBeRenamed.Database
{
    public class PostgresSqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public PostgresSqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration["ConnectionStrings:PostgresConnection"];
        }

        public DbConnection GetSqlConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}