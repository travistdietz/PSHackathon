namespace PSProductService.Models;

public class ProductResponse
{
    public string Question { get; set; }
    public IEnumerable<ProductDto> Products { get; set; }
}