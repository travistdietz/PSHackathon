using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;
using OpenAI.Models;
using PSProductService.Models;
using PSProductService.Repositories;
using PSProductService.Services;

namespace PSProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public class ProductController : Controller
    {
        readonly ILogger<ProductController> _logger;
        readonly IConfiguration _configuration;
        readonly IProductRepository _productRepository;
        readonly IQuestionGenerator _questionGenerator;
        readonly IProductSelector _productSelector;

        public ProductController(ILogger<ProductController> logger, 
            IConfiguration configuration, 
            IProductRepository productRepository, 
            IQuestionGenerator questionGenerator, 
            IProductSelector productSelector)
        {
            _logger = logger;
            _configuration = configuration;
            _productRepository = productRepository;
            _questionGenerator = questionGenerator;
            _productSelector = productSelector;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("")]
        public async Task<ActionResult<string>> GetAll(string eventType)
        {
            return Ok(await GenerateResponse(eventType));
        }

        async Task<ProductResponse> GenerateResponse(string eventType)
        {
            var apiKey = _configuration.GetValue<string>("OpenAI:ApiKey");
            var client = new OpenAI.OpenAIClient(apiKey);
            var products = (await _productRepository.GetRandomProducts()).ToList();

            var question = _questionGenerator.GenerateProductForEvent(eventType, string.Join(",", products.Select(x => x.Name)));

            var messages = new List<Message>
            {
                new Message(Role.User, question)
            };
            var chatRequest = new ChatRequest(messages, Model.GPT3_5_Turbo);
            var result = await client.ChatEndpoint.GetCompletionAsync(chatRequest);

            return await _productSelector.Get(question, result.Choices[0].Message, products);
        }
    }
}

    
