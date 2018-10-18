using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.IO;
using System.Threading.Tasks;
using ToBeRenamed.Factories;
using Xunit;

// Disable test parallelization as we cannot allow a database reset
// to happen in the middle test.
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace ToBeRenamed.Tests
{
    public class DatabaseFixture
    {
        public static readonly IServiceScopeFactory ScopeFactory;
        public static readonly Checkpoint FullResetCheckpoint;

        static DatabaseFixture()
        {
            FullResetCheckpoint = new Checkpoint
            {
                SchemasToInclude = new[] { "plum", "public" },
                DbAdapter = DbAdapter.Postgres
            };

            var configuration = Configure();

            // Use the ToBeRenamed.Tests project config to initialize our Startup.cs
            // TODO: We might want to consider using main project's config
            var startup = new Startup(configuration);
            var services = new ServiceCollection();
            startup.ConfigureServices(services);

            // For some reason we have to set this manually
            services.AddTransient(c => configuration);

            // Use our TestSqlConnectionFactory instead of the default
            services.AddTransient<ISqlConnectionFactory, TestSqlConnectionFactory>();

            // Get our service scope factory
            var provider = services.BuildServiceProvider();
            ScopeFactory = provider.GetService<IServiceScopeFactory>();
        }

        private static IConfiguration Configure()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true)
                .AddUserSecrets<DatabaseFixture>()
                .Build();
        }

        /// <summary>
        /// Sends a MediatR request
        /// </summary>
        /// <returns></returns>
        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetService<IMediator>();
                return await mediator.Send(request);
            }
        }

        /// <summary>
        /// Resets the database's tables using the parameters given by the checkpoint
        /// </summary>
        public static async Task ResetDatabase(Checkpoint checkpoint)
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var factory = scope.ServiceProvider.GetService<ISqlConnectionFactory>();

                using (var cnn = factory.GetSqlConnection())
                {
                    await cnn.OpenAsync();
                    await checkpoint.Reset(cnn);
                }
            }
        }
    }
}