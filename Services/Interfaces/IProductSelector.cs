using PSProductService.Models;

namespace PSProductService.Services.Interfaces;

public interface IProductSelector
{
    Task<ProductResponse> Get(string question, string answer, List<Product> products);
    Task<RefinedProductResponse> Get(string requestRefiningQuestion, string answer, List<Product> products, List<ChatRequest> chats);
}
