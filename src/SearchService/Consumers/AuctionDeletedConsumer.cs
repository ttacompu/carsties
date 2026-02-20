using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        Console.WriteLine($"--> Consuming AuctionDeleted event for auction ID: {context.Message.Id}");
        var result = await DB.Default.DeleteAsync<Item>(context.Message.Id);
        if(!result.IsAcknowledged)
        {
            throw new MessageException(typeof(AuctionDeleted), "Failed to delete item from MongoDB");
        }   
    }
}
