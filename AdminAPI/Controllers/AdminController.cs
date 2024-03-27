using AdminAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace AdminAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoCollection<ProductDetail> _productsCollection;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IMongoClient mongoClient, ILogger<AdminController> logger)
        {
            _mongoClient = mongoClient;
            var database = _mongoClient.GetDatabase("ECommerce");
            _productsCollection = database.GetCollection<ProductDetail>("Products");
            _logger = logger;
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDetail product)
        {
            try
            {
                _logger.LogInformation("Product added successfully");
                await _productsCollection.InsertOneAsync(product);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Some error occured while adding product");
                return StatusCode(500, $"An error occurred while adding the product: {ex.Message}");
            }
        }
        [Authorize(Roles ="Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var result = await _productsCollection.DeleteOneAsync(p => p._id == id);
            if (result.DeletedCount > 0)
            {
                _logger.LogInformation("Product deleted successfully");
                return NoContent();
            }
            else
            {
                _logger.LogInformation($"Product with ID {id} not found.");
                return NotFound($"Product with ID {id} not found.");
            }
        }
    }
}
