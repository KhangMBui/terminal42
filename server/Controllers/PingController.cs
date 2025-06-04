using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Terminal42.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PingController : ControllerBase
    {
        private readonly IMongoDatabase _database;

        public PingController(IMongoDatabase database)
        {
            _database = database;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _database.RunCommandAsync<BsonDocument>(
                    new BsonDocument("ping", 1)
                );
                return Ok(new { message = "✅ Connected to MongoDB!", result });
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new { message = "❌ Failed to connect to MongoDB", error = ex.Message }
                );
            }
        }
    }
}
