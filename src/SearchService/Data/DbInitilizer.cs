using SearchService.Models;
using MongoDB.Driver;
using MongoDB.Entities;
using System.Text.Json;
using SearchService.Services;

namespace SearchService.Data;

public class DbInitializer
{
    public static async Task InitDb(WebApplication app)
    {
        var db = await DB.InitAsync("SearchDb", MongoClientSettings
    .FromConnectionString(app.Configuration.GetConnectionString("MongoDbConnection")));
        await db.Index<Item>()
            .Key(x => x.Make, KeyType.Text)
            .Key(x => x.Model, KeyType.Text)
            .Key(x => x.Color, KeyType.Text)
            .CreateAsync();

        var count = await db.CountAsync<Item>();
        using var scope = app.Services.CreateScope(); 
        var httpClient = scope.ServiceProvider.GetRequiredService<AuctionSvcHttpClient>(); 
        var items = await httpClient.GetItemsForSearchDb(); 

        Console.WriteLine($"Initializing database with {items.Count()} items...");

        if(items.Count() > 0)
        {
            await db.SaveAsync(items);
            Console.WriteLine("Database initialization completed.");
        }
        else
        {
            Console.WriteLine("No new items to initialize in the database.");
        }   
    }
}