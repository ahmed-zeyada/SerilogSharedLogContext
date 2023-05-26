using Microsoft.AspNetCore.Mvc;
using SerilogExtensionDemo.Infrastructure;

namespace SerilogExtensionDemo.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;

        public HomeController(ILogger<HomeController> logger, IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("products/{catId:int}")]
        public async Task<IActionResult> GetProducts(int catId, [FromQuery] bool onlyAvailable)
        {
            _logger.LogInformation("products info requested for category:{CategoryId}, OnlyAvailable:{onlyAvailable}", catId, onlyAvailable);
            var products = await _repository.GetProducts(catId);

            // this log will include the properties added from repository 
            _logger.LogInformation("products count:{ProductsCount}", products.Count);

            return Ok(products);
        }
    }
}