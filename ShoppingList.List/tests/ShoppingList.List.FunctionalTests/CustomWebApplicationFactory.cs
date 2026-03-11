using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ShoppingList.List.Infrastructure.Data;
using System.Collections.Generic;

namespace ShoppingList.List.FunctionalTests;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    public CustomWebApplicationFactory()
    {
        // Set environment variables BEFORE the host is built
        Environment.SetEnvironmentVariable("DatabaseProvider", "Sqlite");
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", $"Data Source={Guid.NewGuid()}.sqlite");
        
        // Mock Auth settings to bypass Guards
        Environment.SetEnvironmentVariable("Authentication__Authority", "http://localhost:8080");
        Environment.SetEnvironmentVariable("Authentication__RequireHttps", "false");
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureTestServices(services =>
        {
            // Replace Authentication with TestAuthHandler
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test";
                options.DefaultChallengeScheme = "Test";
            })
            .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
        });
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = builder.Build();
        host.Start();

        // Get service provider.
        var serviceProvider = host.Services;

        // Create a scope to obtain a reference to the database
        // context (AppDbContext).
        using (var scope = serviceProvider.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<AppDbContext>();

            var logger = scopedServices.GetRequiredService<
                ILogger<CustomWebApplicationFactory<TProgram>>
            >();

            // Ensure the database is created.
            db.Database.EnsureCreated();

            try
            {
                // Seed the database with test data.
                SeedData.InitializeAsync(db).Wait();
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "An error occurred seeding the "
                        + "database with test messages. Error: {exceptionMessage}",
                    ex.Message
                );
            }
        }

        return host;
    }
}
