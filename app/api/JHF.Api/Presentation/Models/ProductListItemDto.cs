namespace JHF.Api.Presentation.Models;
public class ProductListItemDto
{
    public long Id { get; set; }
    public string Name { get; set; } = "";
    public string Slug { get; set; } = "";
    public string Brand { get; set; } = "";
    public decimal Price { get; set; }           // giá hiện tại (ưu tiên sale nếu đang trong khung giờ)
    public bool IsOnSale { get; set; }
    public string? ImageUrl { get; set; }
    public string PreorderStatus { get; set; } = "AVAILABLE";
}
