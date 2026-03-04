using System;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper _mapper;

    public AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        Console.WriteLine($"--> Consuming AuctionUpdated event for auction ID: {context.Message.Id}");
        var item = _mapper.Map<Item>(context.Message);
        var result = await DB.Default.Update<Item>()
            .Match(i => i.ID == context.Message.Id)
            .ModifyOnly(i => new
            {
                i.Make,
                i.Model,
                i.Year,
                i.Mileage,
                i.ImageUrl
            }, item)
            .ExecuteAsync();

        if (!result.IsAcknowledged)
        {
            throw new MessageException(typeof(AuctionUpdated), "Failed to update item in MongoDB");
        }
    }
}
