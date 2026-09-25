using Backend.Data;
using Backend.Models;
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

    // Seed data if tables are empty
    if (!dbContext.Players.Any())
    {
        var player1 = new Player { PlayerId = Guid.NewGuid(), PlayerName = "ShadowBlade", FullName = "Nguyen Van A", Age = "22", Level = 45, Email = "shadow@game.com" };
        var player2 = new Player { PlayerId = Guid.NewGuid(), PlayerName = "LunarMage", FullName = "Tran Thi B", Age = "19", Level = 72, Email = "lunar@game.com" };
        var player3 = new Player { PlayerId = Guid.NewGuid(), PlayerName = "IronFist99", FullName = "Le Van C", Age = "25", Level = 33, Email = "iron@game.com" };
        var player4 = new Player { PlayerId = Guid.NewGuid(), PlayerName = "CyberNinja", FullName = "Pham Thi D", Age = "21", Level = 88, Email = "cyber@game.com" };
        var player5 = new Player { PlayerId = Guid.NewGuid(), PlayerName = "FrostQueen", FullName = "Hoang Van E", Age = "28", Level = 56, Email = "frost@game.com" };
        var player6 = new Player { PlayerId = Guid.NewGuid(), PlayerName = "ThunderGod", FullName = "Vo Thi F", Age = "30", Level = 95, Email = "thunder@game.com" };
        var player7 = new Player { PlayerId = Guid.NewGuid(), PlayerName = "PixelHunter", FullName = "Dang Van G", Age = "17", Level = 15, Email = "pixel@game.com" };
        var player8 = new Player { PlayerId = Guid.NewGuid(), PlayerName = "DarkPhoenix", FullName = "Bui Thi H", Age = "24", Level = 67, Email = "dark@game.com" };

        dbContext.Players.AddRange(player1, player2, player3, player4, player5, player6, player7, player8);
        dbContext.SaveChanges();

        var asset1 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Dragon Slayer Sword", LevelRequire = 40 };
        var asset2 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Phoenix Shield", LevelRequire = 35 };
        var asset3 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Staff of Eternity", LevelRequire = 60 };
        var asset4 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Moonstone Amulet", LevelRequire = 50 };
        var asset5 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Titan Gauntlets", LevelRequire = 30 };
        var asset6 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Plasma Katana", LevelRequire = 75 };
        var asset7 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Stealth Cloak", LevelRequire = 70 };
        var asset8 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Shadow Boots", LevelRequire = 65 };
        var asset9 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Ice Crown", LevelRequire = 50 };
        var asset10 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Blizzard Staff", LevelRequire = 55 };
        var asset11 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Mjolnir Hammer", LevelRequire = 90 };
        var asset12 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Lightning Armor", LevelRequire = 85 };
        var asset13 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Wooden Bow", LevelRequire = 1 };
        var asset14 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Flame Wings", LevelRequire = 60 };
        var asset15 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Rebirth Pendant", LevelRequire = 55 };
        var asset16 = new Asset { AssetId = Guid.NewGuid(), AssetName = "Storm Shield", LevelRequire = 88 };

        dbContext.Assets.AddRange(asset1, asset2, asset3, asset4, asset5, asset6, asset7, asset8, asset9, asset10, asset11, asset12, asset13, asset14, asset15, asset16);
        dbContext.SaveChanges();

        dbContext.PlayerAssets.AddRange(
            // ShadowBlade
            new PlayerAsset { PlayerId = player1.PlayerId, AssetId = asset1.AssetId },
            new PlayerAsset { PlayerId = player1.PlayerId, AssetId = asset2.AssetId },
            // LunarMage
            new PlayerAsset { PlayerId = player2.PlayerId, AssetId = asset3.AssetId },
            new PlayerAsset { PlayerId = player2.PlayerId, AssetId = asset4.AssetId },
            // IronFist99
            new PlayerAsset { PlayerId = player3.PlayerId, AssetId = asset5.AssetId },
            // CyberNinja
            new PlayerAsset { PlayerId = player4.PlayerId, AssetId = asset6.AssetId },
            new PlayerAsset { PlayerId = player4.PlayerId, AssetId = asset7.AssetId },
            new PlayerAsset { PlayerId = player4.PlayerId, AssetId = asset8.AssetId },
            // FrostQueen
            new PlayerAsset { PlayerId = player5.PlayerId, AssetId = asset9.AssetId },
            new PlayerAsset { PlayerId = player5.PlayerId, AssetId = asset10.AssetId },
            // ThunderGod
            new PlayerAsset { PlayerId = player6.PlayerId, AssetId = asset11.AssetId },
            new PlayerAsset { PlayerId = player6.PlayerId, AssetId = asset12.AssetId },
            new PlayerAsset { PlayerId = player6.PlayerId, AssetId = asset16.AssetId },
            // PixelHunter
            new PlayerAsset { PlayerId = player7.PlayerId, AssetId = asset13.AssetId },
            // DarkPhoenix
            new PlayerAsset { PlayerId = player8.PlayerId, AssetId = asset14.AssetId },
            new PlayerAsset { PlayerId = player8.PlayerId, AssetId = asset15.AssetId }
        );
        dbContext.SaveChanges();

        Console.WriteLine("=== SEED DATA INSERTED: 8 players, 16 assets, 16 player-assets ===");
    }
    else
    {
        Console.WriteLine("=== Database already has data, skipping seed ===");
    }
}

host.Run();
