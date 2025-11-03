using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ST10174327_GiftOfTheGiversWebApp.Data;
using System;
using System.Linq;

namespace ST10174327_GiftOfTheGiversWebApp.Tests.IntegrationTests
{
    public class TestBase : IDisposable
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly HttpClient _client;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _databaseName = $"TestDb_{Guid.NewGuid()}";

        public TestBase()
        {
            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove the existing database context registration
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        // Add in-memory database for testing
                        services.AddDbContext<ApplicationDbContext>(options =>
                        {
                            options.UseInMemoryDatabase(_databaseName);
                        });

                        // Build the service provider
                        _serviceProvider = services.BuildServiceProvider();
                    });
                });

            _client = appFactory.CreateClient();
            _dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            _dbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            _client.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
