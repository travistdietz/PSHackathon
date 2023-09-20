using PSProductService.Models;
using System.Text.Json;

namespace PSProductService.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetRandomProducts();
}

public class ProductRepository : IProductRepository
{
    public async Task<IEnumerable<Product>> GetRandomProducts()
    {
        var jsonText = File.ReadAllText("data/products.json");
        return JsonSerializer.Deserialize<List<Product>>(jsonText);
    }
}