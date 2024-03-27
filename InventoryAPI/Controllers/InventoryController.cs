using InventoryAPI.Model;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InventoryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : Controller
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoCollection<Product> _productsCollection;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(IMongoClient mongoClient, ILogger<InventoryController> logger)
        {
            _mongoClient = mongoClient;
            var database = _mongoClient.GetDatabase("ECommerce");
            _productsCollection = database.GetCollection<Product>("Products");
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _productsCollection.Find(_ => true).ToListAsync();
            _logger.LogInformation("Products fetched successfully");
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> GetProductById([FromBody] string id)
        {
            var response = await _productsCollection.Find(p => p._id==id).FirstOrDefaultAsync();
            if (response != null)
            {
                _logger.LogInformation("Products fetched successfully");
                return Ok(response);
            }
            else
            {
                _logger.LogInformation($"Product with ID {id} not found.");
                return NotFound($"Product with ID {id} not found.");
            }
            
        } 
    }
}
