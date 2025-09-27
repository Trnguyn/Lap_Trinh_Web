using JHF.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JHF.Api.Domain.Entities;
using JHF.Api.Presentation.Models;

namespace JHF.Api.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) => _db = db;

    // GET /api/products?page=1&pageSize=12
    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int page = 1, [FromQuery] int pageSize = 12)
    {
        if (page < 1) page = 1;
        if (pageSize is <= 0 or > 100) pageSize = 12;

        var now = DateTimeOffset.UtcNow;

        var q =
            from p in _db.Set<Product>() // app_schema.products
            join b in _db.Set<Brand>() on p.BrandId equals b.Id into jb
            from b in jb.DefaultIfEmpty()

            // ảnh cover
            let cover = _db.Set<ProductImage>()
                           .Where(i => i.ProductId == p.Id)
                           .OrderBy(i => i.SortOrder)
                           .Select(i => i.ImageUrl)
                           .FirstOrDefault()

            // giá đang sale (nullable)
            let salePrice = _db.Set<ProductVariant>()
                               .Where(v => v.ProductId == p.Id
                                        && v.IsActive
                                        && v.SaleStartsAt != null
                                        && v.SaleEndsAt   != null
                                        && v.SaleStartsAt <= now
                                        && v.SaleEndsAt   >= now)
                               .OrderBy(v => v.Price)
                               .Select(v => (decimal?)v.Price)
                               .FirstOrDefault()

            // giá thường (nullable)
            let basePrice = _db.Set<ProductVariant>()
                               .Where(v => v.ProductId == p.Id && v.IsActive)
                               .OrderBy(v => v.Price)
                               .Select(v => (decimal?)v.Price)
                               .FirstOrDefault()

            select new ProductListItemDto
            {
                Id = p.Id,
                Name = p.Name,
                Slug = p.Slug,
                Brand = b != null ? b.Name : "",
                ImageUrl = cover,
                Price = salePrice ?? basePrice ?? 0m,
                IsOnSale = salePrice != null,
                PreorderStatus = EF.Property<string>(p, "preorder_status")
            };

        var total = await q.CountAsync();
        var items = await q.OrderBy(x => x.Id)
                           .Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToListAsync();

        return Ok(new { total, page, pageSize, items });
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> One(long id)
    {
        var dto = await (
            from p in _db.Set<Product>()
            where p.Id == id
            let cover = _db.Set<ProductImage>()
                           .Where(i => i.ProductId == p.Id)
                           .OrderBy(i => i.SortOrder)
                           .Select(i => i.ImageUrl)
                           .FirstOrDefault()
            select new {
                p.Id, p.Name, p.Slug, p.Description,
                Cover = cover
            }
        ).FirstOrDefaultAsync();

        return dto is null ? NotFound() : Ok(dto);
    }
}
