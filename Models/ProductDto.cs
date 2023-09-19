namespace PSProductService.Models;

public class ProductDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public string Vendor { get; set; }
    public string ImageUrl { get; set; }
    public string EventSpecificDescription { get; set; }
}