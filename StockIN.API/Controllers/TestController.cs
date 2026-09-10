//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using StockIN.API.Data;

//namespace StockIN.API.Controllers;

//[ApiController]
//[Route("api/test")]
//public class TestController : ControllerBase
//{
//    private readonly AppDbContext _context;

//    public TestController(AppDbContext context)
//    {
//        _context = context;
//    }

//    [HttpGet("categories")]
//    public async Task<IActionResult> GetCategories()
//    {
//        var categories = await _context.Categories
//            .Take(5)
//            .ToListAsync();

//        return Ok(categories);
//    }
// }
// al ejecutar https://localhost:7143/api/test/categories me mestras los valores de la tabla categories "products":[] aparece asi ya que por el scaffolding detecta que es una FK