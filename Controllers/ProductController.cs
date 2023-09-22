using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        readonly IProductRepository _productRepository;
        readonly IQuestionGenerator _questionGenerator;
        readonly IProductSelector _productSelector;
        readonly IAiService _aiService;

        public ProductController(ILogger<ProductController> logger,
            IProductRepository productRepository, 
            IQuestionGenerator questionGenerator, 
            IProductSelector productSelector, 
            IAiService aiService)
        {
            _logger = logger;
            _productRepository = productRepository;
            _questionGenerator = questionGenerator;
            _productSelector = productSelector;
            _aiService = aiService;
        }

        /// <summary>
        /// Returns promotional products for a specific event
        /// </summary>
        /// <param name="eventDescription">Describe the event you are looking to seed with promotional products</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("EventPost")]
        public async Task<ActionResult<ProductResponse>> EventPost(string eventDescription)
        {
            try
            {
                var products = (await _productRepository.GetRandomProducts()).ToList();
                var question = _questionGenerator.GenerateProductForEvent(eventDescription, string.Join(",", products.Select(x => x.Name)));
                var answer = await _aiService.AskQuestion(question);
                var response = await _productSelector.Get(question, answer, products);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Ok($"We could not find products based on that criteria: {e.Message}");
            }
        }

        /// <summary>
        /// Returns promotional products further refined from original question posted for a specific event
        /// </summary>
        /// <param name="RefindChatRequest">Contains previous chat context up to this point and the
        /// question to further refine the promotional products that are being selected</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("RefinedPost")]
        public async Task<ActionResult<RefinedProductResponse>> RefinedPost(RefinedChatRequest request)
        {
            try
            {
                var products = (await _productRepository.GetRandomProducts()).ToList();
                var question = _questionGenerator.GenerateRefinedQuestion(request.RefiningQuestion);
                var answer = await _aiService.AskQuestionWithPreviousContext(question, request.ChatLog);
                var response = await _productSelector.Get(question, answer, products, request.ChatLog);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Ok($"We could not find products based on that criteria: {e.Message}");
            }
        }
    }
}

    
