﻿using Microsoft.AspNetCore.Authorization;
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

        [AllowAnonymous]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("")]
        public async Task<ActionResult<ProductResponse>> GetAll(string eventType)
        {
            try
            {
                var products = (await _productRepository.GetRandomProducts()).ToList();
                var question = _questionGenerator.GenerateProductForEvent(eventType, string.Join(",", products.Select(x => x.Name)));
                var answer = await _aiService.AskQuestion(question);
                var response = await _productSelector.Get(question, answer, products);
                return Ok(response);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Ok(e.Message);
            }
        }
    }
}

    
