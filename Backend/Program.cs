using Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
        if (string.IsNullOrEmpty(connectionString))
        {
            // Fallback for local development if needed, though Docker will provide it
            connectionString = "Server=localhost;Database=BATTLEGAME;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;";
        }
        services.AddDbContext<GameDbContext>(options =>
            options.UseSqlServer(connectionString));
    })
    .Build();
using (var scope = host.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<GameDbContext>();
    dbContext.Database.Migrate();
}

host.Run();
