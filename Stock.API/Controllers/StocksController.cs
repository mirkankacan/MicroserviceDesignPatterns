using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stock.API.Models;

namespace Stock.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController(StockDbContext dbContext) : ControllerBase
    {
        [HttpGet]
        public async Task<IResult> GetStocks()
        {
            return Results.Ok(await dbContext.Stocks.ToListAsync());
        }
    }
}