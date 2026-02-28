using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;


namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
       

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Item>>> Search( [FromQuery] SearchParams searchParams)
        {
            var query = DB.Default.PagedSearch<Item, Item>();
            
            if(!string.IsNullOrEmpty(searchParams.SearchTerm))
            {
                query.Match(b => b.Text(searchParams.SearchTerm)).SortByTextScore();
            }

            query = searchParams.OrderBy switch
            {
                "make" => query.Sort(x => x.Ascending(x => x.Make)).Sort(x => x.Ascending(a =>a.Model )),
                "new" => query.Sort(x => x.Descending(x => x.CreatedAt)),
                _ => query.Sort(x => x.Ascending(x => x.AuctionEnd))
            };

            query = searchParams.FilterBy switch
            {
                
                "finished" => query.Match(x => x.Lt(i => i.AuctionEnd, DateTime.UtcNow)),
                "endingSoon" => query.Match(x => x.Lt(i => i.AuctionEnd, DateTime.UtcNow.AddHours(6)))
                .Match(x => x.Gt(i => i.AuctionEnd, DateTime.UtcNow)),
                _ => query.Match(x => x.Gt(i => i.AuctionEnd, DateTime.UtcNow)),
            };

            if(!string.IsNullOrEmpty(searchParams.Seller))
            {
                query.Match(x => x.Eq(i => i.Seller, searchParams.Seller));
            }

            if(!string.IsNullOrEmpty(searchParams.Winner))
            {
                query.Match(x => x.Eq(i => i.Winner, searchParams.Winner));
            }

            query.PageNumber(searchParams.PageNumber);
            query.PageSize(searchParams.PageSize);   
            var result = await query.ExecuteAsync();
            return Ok(new { results = result.Results, pageCount = result.PageCount, totalCount = result.TotalCount  });
        }
    }
}