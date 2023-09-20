namespace PSProductService.Models;

public class ProductResponse
{
    public string Question { get; set; }
    public string Answer { get; set; }
    public IEnumerable<ProductDto> Products { get; set; }
}