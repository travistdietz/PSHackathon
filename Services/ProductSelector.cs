using System.Text.Json;
using PSProductService.Models;

namespace PSProductService.Services;

public interface IProductSelector
{
    Task<ProductResponse> Get(string question, string result, List<Product> products);
}

public class ProductSelector : IProductSelector
{
    public async Task<ProductResponse> Get(string question, string result, List<Product> products)
    {
        var productResponses = JsonSerializer.Deserialize<AIResponse>(result);
        var productDtos = new List<ProductDto>();

        foreach (var productResponse in productResponses.products)
        {
            var product = products.Single(x => x.Name == productResponse.name);
            productDtos.Add(new ProductDto
            {
                Description = product.Description,
                Name = product.Name,
                EventSpecificDescription = productResponse.description,
                ImageUrl = product.ImageUrl,
                Price = product.Price,
                Vendor = product.Vendor
            });
        }

        return new ProductResponse
        {
            Question = question,
            Products = productDtos
        };
    }

    public class AIProduct
    {
        public string name { get; set; }
        public string description { get; set; }
    }

    public class AIResponse
    {
        public List<AIProduct> products { get; set; }
    }
}