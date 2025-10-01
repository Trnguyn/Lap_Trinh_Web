using JHF.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JHF.Api.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // trỏ đúng schema trong Postgres mà bạn đã tạo
        b.HasDefaultSchema("app_schema");

        b.Entity<Product>(e =>
        {
            e.ToTable("products");            // => app_schema.products
            e.HasKey(x => x.Id);

            // map cột 1-1
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Name).HasColumnName("name");
            e.Property(x => x.Slug).HasColumnName("slug");
            e.Property(x => x.Description).HasColumnName("description");
            e.Property(x => x.BrandId).HasColumnName("brand_id");
            e.Property<string>("preorder_status").HasColumnName("preorder_status");
            e.Property<DateTimeOffset?>("created_at").HasColumnName("created_at");
        });

        b.Entity<Brand>(e =>
        {
            e.ToTable("brands");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.Name).HasColumnName("name");
        });

        b.Entity<ProductImage>(e =>
        {
            e.ToTable("product_images");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.ProductId).HasColumnName("product_id");
            e.Property(x => x.ImageUrl).HasColumnName("image_url");
            e.Property(x => x.SortOrder).HasColumnName("sort_order");
        });

        b.Entity<ProductVariant>(e =>
        {
            e.ToTable("product_variants");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).HasColumnName("id");
            e.Property(x => x.ProductId).HasColumnName("product_id");
            e.Property(x => x.Sku).HasColumnName("sku");
            e.Property(x => x.Price).HasColumnName("price").HasPrecision(12, 2);
            e.Property(x => x.CompareAtPrice).HasColumnName("compare_at_price").HasPrecision(12, 2);
            e.Property(x => x.SaleStartsAt).HasColumnName("sale_starts_at");
            e.Property(x => x.SaleEndsAt).HasColumnName("sale_ends_at");
            e.Property(x => x.IsActive).HasColumnName("is_active");
        });

        base.OnModelCreating(b);
    }
}
