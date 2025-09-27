namespace JHF.Api.Domain.Entities;

public class Product
{
    // Map 1-1 với bảng app_schema.products
    public long Id { get; set; }              // BIGSERIAL -> long
    public string Name { get; set; } = "";    // name
    public string Slug { get; set; } = "";    // slug
    public string? Description { get; set; }  // description
    public long? BrandId { get; set; }        // brand_id (có thể null)
}
