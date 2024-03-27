using CartAPI.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : Controller
    {
        private readonly IMongoClient _mongoClient;
        private readonly IMongoCollection<Cart> _cartsCollection;
        private readonly IMongoCollection<Cart> _checkoutCollection;
        private readonly ILogger<CartController> _logger;

        public CartController(IMongoClient mongoClient, ILogger<CartController> logger)
        {
            _mongoClient = mongoClient;
            var database = _mongoClient.GetDatabase("ECommerce");
            _cartsCollection = database.GetCollection<Cart>("Cart");
            _checkoutCollection = database.GetCollection<Cart>("Checkout");
            _logger = logger;
            
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(string userName)
        {
            var response = await _cartsCollection.Find(c=>c.userName==userName).ToListAsync();
            // var response = await ReadInventoryDataAsync();
            _logger.LogInformation("Cart fetched successfully");
            return Ok(response);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] Cart cartItem)
        {
            try
            {
                _logger.LogInformation("Product added successfully to cart");
                var response = await _cartsCollection.Find(c=>c.userName == cartItem.userName && c.productId==cartItem.productId).FirstOrDefaultAsync();
                if (response == null)
                {
                    await _cartsCollection.InsertOneAsync(cartItem);
                }
                else
                {
                    response.quantity += cartItem.quantity;
                    await _cartsCollection.ReplaceOneAsync(c=>c._id == response._id,response);
                }
                return Ok(cartItem);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Some error occured while adding product to cart");
                return StatusCode(500, $"An error occurred while adding the product to cart: {ex.Message}");
            }
        }


        [Authorize(Roles = "User")]
        [HttpDelete]
        public async Task<IActionResult> Checkout( string id)
        {
            try
            {
                _logger.LogInformation("Checkout successfully done");
                var response = await _cartsCollection.Find(c => c._id== id).FirstOrDefaultAsync();
                var result = await _cartsCollection.DeleteOneAsync(p => p._id ==id);
                if (result.DeletedCount > 0)
                {
                    _logger.LogInformation("Checkout successful");
                    await _checkoutCollection.InsertOneAsync(response);
                    return Ok(true);
                }
                else
                {
                    _logger.LogInformation($"Product with ID {id} not found.");
                    return NotFound($"Product with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Some error occured while adding product to cart");
                return StatusCode(500, $"An error occurred while adding the product to cart: {ex.Message}");
            }
        }
    }
}
