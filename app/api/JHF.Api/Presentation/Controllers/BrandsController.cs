using JHF.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JHF.Api.Domain.Entities;

namespace JHF.Api.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BrandsController(AppDbContext db) : ControllerBase
{
    // GET /api/brands
    [HttpGet]
    public async Task<IActionResult> List()
    {
        var items = await db.Set<Brand>()
            .AsNoTracking()
            .OrderBy(b => b.Name)
            .Select(b => new { b.Id, b.Name })
            .ToListAsync();
        return Ok(items);
    }
}
