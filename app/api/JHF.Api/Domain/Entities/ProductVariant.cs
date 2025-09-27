namespace JHF.Api.Domain.Entities;

public class ProductVariant
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string? Sku { get; set; }
    public decimal Price { get; set; }
    public decimal? CompareAtPrice { get; set; }
    // TIMESTAMPTZ -> dùng DateTimeOffset?
    public DateTimeOffset? SaleStartsAt { get; set; }
    public DateTimeOffset? SaleEndsAt { get; set; }
    public bool IsActive { get; set; }
}
