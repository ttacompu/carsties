using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;

namespace AuctionService.RequestHelpers
{
    // AutoMapper profile that wires Request DTOs to Domain models and Domain models to Response DTOs.
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            
            CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
            CreateMap<Item, AuctionDto>();

            CreateMap<CreateAuctionDto, Auction>()
                .ForMember(dest => dest.Item, opt => opt.MapFrom(src => src));

            CreateMap<CreateAuctionDto, Item>();
               
        }
    }
}