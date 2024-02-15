using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WPInventory.BL.Settings;
using WPInventory.Data;
using WPInventory.Models;

namespace WPInventory.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SettingsController : MediatorController
    {
        private readonly ComputerInfoContext _dbContext;
        private readonly ILogger<ComputerInfoContext> _logger;
        private readonly IConfiguration _config;

        public SettingsController(ComputerInfoContext dbContext, ILogger<ComputerInfoContext> logger, IConfiguration config)
        {
            _dbContext = dbContext;
            _logger = logger;
            _config = config;
        }

        [HttpGet("SearchScopes")]
        public async Task<ActionResult<GetScopesResult>> GetSearchScopes()
        {
            var request = new GetScopesRequest();
            var result = await Mediator.Send(request);

            return result;
        }

        [HttpPut("SearchScopes/{id:int}")]
        public async Task<ActionResult> UpdateSearchScope([FromRoute][Range(1,int.MaxValue)] int id,[FromBody]UpdateScopeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request model");
            }

            var request = new UpdateScopeRequest()
            {
                Id = id,
                IsEnabled = model.IsEnabled,
                ScopePath = model.ScopePath
            };

            return await Mediator.Send(request);
        }

        [HttpPost("SearchScopes")]
        public async Task<ActionResult> AddSearchScope([FromBody]CreateScopeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid request model");
            }

            var request = new CreateScopeRequest()
            {
                IsEnabled = model.IsEnabled,
                ScopePath = model.ScopePath
            };

            return await Mediator.Send(request);
        }

        [HttpDelete("SearchScopes/{id:int}")]
        public async Task<ActionResult> DeleteSearchScope([FromRoute] int id)
        {
            var request = new DeleteScopeRequest()
            {
                Id = id
            };

            return await Mediator.Send(request);
        }
    }
}
