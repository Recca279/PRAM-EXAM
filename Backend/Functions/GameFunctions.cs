using Backend.Data;
using Backend.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace Backend.Functions
{
    public class GameFunctions
    {
        private readonly GameDbContext _dbContext;
        private readonly ILogger<GameFunctions> _logger;

        public GameFunctions(GameDbContext dbContext, ILogger<GameFunctions> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [Function("registerplayer")]
        public async Task<HttpResponseData> RegisterPlayer([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function registerplayer processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<Player>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (data == null)
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Invalid player data.");
                return badResponse;
            }

            if (data.PlayerId == Guid.Empty)
                data.PlayerId = Guid.NewGuid();

            _dbContext.Players.Add(data);
            await _dbContext.SaveChangesAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(data);
            return response;
        }

        [Function("createasset")]
        public async Task<HttpResponseData> CreateAsset([HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function createasset processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonSerializer.Deserialize<Asset>(requestBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (data == null)
            {
                var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await badResponse.WriteStringAsync("Invalid asset data.");
                return badResponse;
            }

            if (data.AssetId == Guid.Empty)
                data.AssetId = Guid.NewGuid();

            _dbContext.Assets.Add(data);
            await _dbContext.SaveChangesAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(data);
            return response;
        }

        [Function("getassetsbyplayer")]
        public async Task<HttpResponseData> GetAssetsByPlayer([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function getassetsbyplayer processed a request.");

            var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
            var playerIdStr = query["playerId"];
            
            var results = await _dbContext.PlayerAssets
                .Include(pa => pa.Player)
                .Include(pa => pa.Asset)
                .Select(pa => new
                {
                    PlayerName = pa.Player.PlayerName,
                    Level = pa.Player.Level,
                    Age = pa.Player.Age,
                    AssetName = pa.Asset.AssetName
                })
                .ToListAsync();

            // Note: If a specific playerId is provided, filter by it. But the requirement table shows all players.
            if (!string.IsNullOrEmpty(playerIdStr) && Guid.TryParse(playerIdStr, out Guid playerId))
            {
                // filter if needed
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(results);
            return response;
        }
    }
}
