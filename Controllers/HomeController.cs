using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAI.Chat;
using OpenAI.Models;

namespace PSProductService.Controllers
{
    [ApiController]
    [Route("/")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public class ProductController : Controller
    {
        readonly ILogger<ProductController> _logger;
        readonly IConfiguration _configuration;

        public ProductController(ILogger<ProductController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{eventType}")]
        public async Task<ActionResult<string>> GetAll(string eventType)
        {
            return Ok(await GenerateResponse(eventType));
        }

        async Task<string> GenerateResponse(string eventType)
        {
            var apiKey = _configuration.GetValue<string>("OpenAI:ApiKey");
            var client = new OpenAI.OpenAIClient(apiKey);

            var messages = new List<Message>
            {
                new Message(Role.User, eventType)
            };
            var chatRequest = new ChatRequest(messages, Model.GPT3_5_Turbo);
            var result = await client.ChatEndpoint.GetCompletionAsync(chatRequest);

            return result.Choices[0].Message;
        }
    }
}
