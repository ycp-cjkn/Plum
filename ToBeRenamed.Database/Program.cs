using System;
using System.IO;
using System.Reflection;
using Dapper;
using DbUp;
using DbUp.Engine;
using Microsoft.Extensions.Configuration;

namespace ToBeRenamed.Database
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            
            if (args.Length != 0 && !string.IsNullOrEmpty(args[0]) && args[0].Equals("delete"))
            {
                var connFactory = new PostgresSqlConnectionFactory(configuration);

                const string dropDatabases = @"
                    DROP DATABASE IF EXISTS ""Plum"";
                    DROP DATABASE IF EXISTS ""TestPlum"";";

                using (var cnn = connFactory.GetSqlConnection())
                {
                    // Insert new user, then get the user id
                    cnn.Execute(dropDatabases);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Databases successfully dropped!");
                    Console.ResetColor();
                }
            }

            var connectionString = configuration["ConnectionStrings:DefaultConnection"];
            var testConnectionString = configuration["ConnectionStrings:TestConnection"];

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(testConnectionString))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: No default or test connection string provided");
                Console.ResetColor();
#if DEBUG
                Console.ReadLine();
#endif
                return -1;
            }

            EnsureDatabase.For.PostgresqlDatabase(connectionString);
            EnsureDatabase.For.PostgresqlDatabase(testConnectionString);

            var upgrader = BuildUpgradeEngineFromConnectionString(connectionString);

            var testUpgrader = BuildUpgradeEngineFromConnectionString(testConnectionString);

            var result = upgrader.PerformUpgrade();
            var testResult = testUpgrader.PerformUpgrade();

            if (!result.Successful)
            {
                WriteResultErrorToConsole(result);
            }

            if (!testResult.Successful)
            {
                WriteResultErrorToConsole(testResult);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();

#if DEBUG
            Console.ReadLine();
#endif

            return 0;
        }

        private static void WriteResultErrorToConsole(DatabaseUpgradeResult result)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(result.Error);
            Console.ResetColor();
#if DEBUG
            Console.ReadLine();
#endif
        }

        private static UpgradeEngine BuildUpgradeEngineFromConnectionString(string connectionString)
        {
            return DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();
        }
    }
}
