using System.Net.Http.Json;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services
{
    public class AuctionSvcHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public AuctionSvcHttpClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Item>> GetItemsForSearchDb()
        {
            try
            {
                var lastItem = await DB.Default.Find<Item>()
                    .Sort(x => x.Descending(i => i.UpdatedAt))
                    .ExecuteFirstAsync();

                var lastUpdated = lastItem?.UpdatedAt.ToString("o") ?? string.Empty;

                return await _httpClient.GetFromJsonAsync<IEnumerable<Item>>(_configuration["AuctionServiceUrl"] + $"/api/auctions?date={lastUpdated}") ?? new List<Item>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Failed to get auction data", ex);
            }
        }
    }
}