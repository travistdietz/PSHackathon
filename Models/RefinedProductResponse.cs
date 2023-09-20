namespace PSProductService.Models;

public class RefinedProductResponse 
{
    public List<ChatRequest> ChatLog { get; set; }
    public IEnumerable<ProductDto> Products { get; set; }
}