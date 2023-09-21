using System.Text.Json;
using DuoVia.FuzzyStrings;
using PSProductService.Models;

namespace PSProductService.Services;

public interface IProductSelector
{
    Task<ProductResponse> Get(string question, string answer, List<Product> products);
    Task<RefinedProductResponse> Get(string requestRefiningQuestion, string answer, List<Product> products, List<ChatRequest> chats);
}

public class ProductSelector : IProductSelector
{
    public async Task<ProductResponse> Get(string question, string answer, List<Product> products)
    {
        var productDtos = GetProducts(answer, products);

        return new ProductResponse
        {
            Question = question,
            Answer = answer,
            Products = productDtos
        };
    }

    public async Task<RefinedProductResponse> Get(string question, string answer, List<Product> products, List<ChatRequest> chats)
    {
        var productDtos = GetProducts(answer, products);

        chats.Add(new ChatRequest
        {
            IsQuestion = true,
            Text = question
        });
        chats.Add(new ChatRequest
        {
            IsQuestion = false,
            Text = answer
        });

        return new RefinedProductResponse
        {
            ChatLog = chats,
            Products = productDtos
        };
    }

    IEnumerable<ProductDto> GetProducts(string answer, List<Product> products)
    {
        var productResponses = JsonSerializer.Deserialize<AIResponse>(answer);
        var productDtos = new List<ProductDto>();

        try
        {
            foreach (var productResponse in productResponses.products)
            {
                var names = products.Select(x => x.Name).ToArray();
                var mostSimilar = names.OrderBy(s => s.LevenshteinDistance(productResponse.name)).First();
                var product = products.First(x => x.Name == mostSimilar);
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

            return productDtos;
        }
        catch (Exception e)
        {
            throw new Exception($"{e.Message} - {answer}");
        }

    }
}