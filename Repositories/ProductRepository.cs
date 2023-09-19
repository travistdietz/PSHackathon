using PSProductService.Models;

namespace PSProductService.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetRandomProducts();
}

public class ProductRepository : IProductRepository
{
    public async Task<IEnumerable<Product>> GetRandomProducts()
    {
        return new List<Product>
        {
            new() { Description = "product #1", Name = "Nike Polo", ImageUrl = "image.jpg", Price = 15, Vendor = "test"},
            new() { Description = "product #2", Name = "Stress Ball", ImageUrl = "image.jpg", Price = 15, Vendor = "test"},
            new() { Description = "product #3", Name = "Fidget Spinner", ImageUrl = "image.jpg", Price = 15, Vendor = "test"},
            new() { Description = "product #4", Name = "Coaster", ImageUrl = "image.jpg", Price = 15, Vendor = "test"},
            new() { Description = "product #5", Name = "Yeti Mug", ImageUrl = "image.jpg", Price = 15, Vendor = "test"}
        };
    }
}