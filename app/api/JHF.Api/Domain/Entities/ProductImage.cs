namespace JHF.Api.Domain.Entities;
public class ProductImage
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public string ImageUrl { get; set; } = "";
    public int SortOrder { get; set; }
}
