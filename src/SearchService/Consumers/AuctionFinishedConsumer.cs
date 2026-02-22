using System;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
{
    
    public async Task Consume(ConsumeContext<AuctionFinished> context)
    {
      var auction= await DB.Default.Find<Item>().OneAsync(context.Message.AuctionId); 
      if(auction != null)
      {
        if (context.Message.ItemSold)
        {
            auction.Winner = context.Message.Winner;
            auction.SoldAmount = context.Message.Amount ?? 0;
        }
        auction.Status = "Finished";
        await DB.Default.SaveAsync(auction);
      }
    }
}
