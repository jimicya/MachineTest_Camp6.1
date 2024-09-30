using AMSWebApi.Models;
using AMSWebApi.Repository;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AMSWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
       
            private readonly IAssetRepository _assetRepository;

            public AssetsController(IAssetRepository assetRepository)
            {
                _assetRepository = assetRepository;
            }
        // GET: api/asset/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssetById(int id)
        {
            var asset = await _assetRepository.GetAssetByIdAsync(id);
            if (asset == null)
            {
                return NotFound($"Asset with ID {id} not found.");
            }
            return Ok(asset);
        }


        // POST: api/asset
        [HttpPost]
            public async Task<IActionResult> CreateAsset([FromBody] AssetMaster asset)
            {
                if (asset == null)
                {
                    return BadRequest("Asset cannot be null.");
                }

                var createdAsset = await _assetRepository.CreateAssetAsync(asset);
                return CreatedAtAction(nameof(GetAssetById), new { id = createdAsset.AmId }, createdAsset);
            }

           

            // GET: api/asset
            [HttpGet]
            public async Task<ActionResult<IEnumerable<AssetMaster>>> GetAllAssets()
            {
                var assets = await _assetRepository.GetAllAssetsAsync();
                return Ok(assets);
            }

            // PUT: api/asset/{id}
            [HttpPut("{id}")]
            public async Task<IActionResult> UpdateAsset(int id, [FromBody] AssetMaster asset)
            {
                if (id != asset.AmId)
                {
                    return BadRequest("Asset ID mismatch.");
                }

                var updated = await _assetRepository.UpdateAssetAsync(asset);
                if (!updated)
                {
                    return NotFound($"Asset with ID {id} not found.");
                }
                return NoContent();
            }


            // GET: api/asset/search?term={searchTerm}
            [HttpGet("search")]
            public async Task<ActionResult<IEnumerable<AssetMaster>>> SearchAssets(string searchTerm)
            {
                var assets = await _assetRepository.SearchAssetsAsync(searchTerm);
                return Ok(assets);
            }
        
    /*
    // GET: api/<AssetsController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<AssetsController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<AssetsController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<AssetsController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<AssetsController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
    */
  }
}
