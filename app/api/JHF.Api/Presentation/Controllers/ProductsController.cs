using JHF.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JHF.Api.Domain.Entities;

namespace JHF.Api.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) => _db = db;

    // GET /api/products?search=&brandId=&sort=&page=&pageSize=
    // sort: price-asc | price-desc | newest
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 12,
        [FromQuery] string? search = null,
        [FromQuery] long? brandId = null,
        [FromQuery] string? sort = null
    )
    {
        if (page < 1) page = 1;
        if (pageSize is <= 0 or > 100) pageSize = 12;

        var now = DateTimeOffset.UtcNow;

        var q =
            from p in _db.Set<Product>()
            join b in _db.Set<Brand>() on p.BrandId equals b.Id into jb
            from b in jb.DefaultIfEmpty()

            let cover = _db.Set<ProductImage>()
                           .Where(i => i.ProductId == p.Id)
                           .OrderBy(i => i.SortOrder)
                           .Select(i => i.ImageUrl)
                           .FirstOrDefault()

            let salePrice = _db.Set<ProductVariant>()
                               .Where(v => v.ProductId == p.Id
                                        && v.IsActive
                                        && v.SaleStartsAt != null
                                        && v.SaleEndsAt != null
                                        && v.SaleStartsAt <= now
                                        && v.SaleEndsAt >= now)
                               .OrderBy(v => v.Price)
                               .Select(v => (decimal?)v.Price)
                               .FirstOrDefault()

            let basePrice = _db.Set<ProductVariant>()
                               .Where(v => v.ProductId == p.Id && v.IsActive)
                               .OrderBy(v => v.Price)
                               .Select(v => (decimal?)v.Price)
                               .FirstOrDefault()

            select new
            {
                id = p.Id,
                name = p.Name,
                slug = p.Slug,
                brandId = p.BrandId,
                brand = b != null ? b.Name : "",
                imageUrl = cover,
                price = salePrice ?? basePrice ?? 0m,
                isOnSale = salePrice != null,
                preorderStatus = EF.Property<string>(p, "preorder_status"),
                createdAt = EF.Property<DateTimeOffset?>(p, "created_at")
            };

        // SEARCH
        if (!string.IsNullOrWhiteSpace(search))
        {
            var key = $"%{search.Trim()}%";
            q = q.Where(x =>
                EF.Functions.ILike(x.name, key) ||
                EF.Functions.ILike(x.slug, key) ||
                EF.Functions.ILike(x.brand, key));
        }

        // FILTER
        if (brandId.HasValue)
            q = q.Where(x => x.brandId == brandId.Value);

        var total = await q.CountAsync();

        // SORT
        q = sort switch
        {
            "price-asc" => q.OrderBy(x => x.price).ThenBy(x => x.id),
            "price-desc" => q.OrderByDescending(x => x.price).ThenBy(x => x.id),
            "newest" => q.OrderByDescending(x => x.createdAt).ThenByDescending(x => x.id),
            _ => q.OrderBy(x => x.id)
        };

        var items = await q.Skip((page - 1) * pageSize)
                           .Take(pageSize)
                           .ToListAsync();

        return Ok(new { total, page, pageSize, items });
    }

    // GET /api/products/{id}
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
            select new
            {
                id = p.Id,
                name = p.Name,
                slug = p.Slug,
                description = p.Description,
                cover
            }
        ).FirstOrDefaultAsync();

        return dto is null ? NotFound() : Ok(dto);
    }

    // GET /api/products/by-slug/{slug}
    // GET /api/products/by-slug/{slug}
[HttpGet("by-slug/{slug}")]
public async Task<IActionResult> DetailBySlug(string slug)
{
    var now = DateTimeOffset.UtcNow;

    // 1) Lấy thông tin cơ bản
    var baseInfo = await (
        from p in _db.Set<Product>()
        join b in _db.Set<Brand>() on p.BrandId equals b.Id into jb
        from b in jb.DefaultIfEmpty()
        where p.Slug == slug
        select new
        {
            p.Id,
            p.Name,
            p.Slug,
            p.Description,
            Brand = b != null ? b.Name : "",
            PreorderStatus = EF.Property<string>(p, "preorder_status")
        }
    ).AsNoTracking().FirstOrDefaultAsync();

    if (baseInfo is null) return NotFound();

    // 2) Ảnh (query riêng)
    var images = await _db.Set<ProductImage>()
        .Where(i => i.ProductId == baseInfo.Id)
        .OrderBy(i => i.SortOrder)
        .Select(i => i.ImageUrl)
        .ToListAsync();

    // 3) Variants (query riêng)
    var variants = await _db.Set<ProductVariant>()
        .Where(v => v.ProductId == baseInfo.Id && v.IsActive)
        .Select(v => new
        {
            id = v.Id,
            sku = v.Sku,
            price = v.Price,
            compareAt = v.CompareAtPrice,
            saleStartsAt = v.SaleStartsAt,
            saleEndsAt = v.SaleEndsAt,
            isOnSale = v.SaleStartsAt != null && v.SaleEndsAt != null
                       && v.SaleStartsAt <= now && v.SaleEndsAt >= now
        })
        .AsNoTracking()
        .ToListAsync();

    var lowestPrice = variants.Count > 0 ? variants.Min(v => v.price) : 0m;

    return Ok(new
    {
        id = baseInfo.Id,
        name = baseInfo.Name,
        slug = baseInfo.Slug,
        description = baseInfo.Description,
        brand = baseInfo.Brand,
        preorderStatus = baseInfo.PreorderStatus,
        images,
        variants,
        lowestPrice
    });
}
}