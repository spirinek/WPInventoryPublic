using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WPInventory.BL.Computers;
using WPInventory.Data;
using WPInventory.Data.Models.Entities;
using WPInventory.Models;

namespace WPInventory.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ComputersController : MediatorController
    {
        private readonly ComputerInfoContext _dbContext;

        public ComputersController(ComputerInfoContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet("All")]
        public async Task <List<Computer>> All()
        {
            var computers = await _dbContext.Computers.Where(x => x.IsArchived == false).ToListAsync();
            return computers;
        }
        [HttpGet("SimpleAll")]
        public async Task<ActionResult<GetAllComputersSimpleModelResult>> SimpleAll([FromQuery]ComputerFilter filter)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("filterError");
            }
            var request = new GetAllComputersSimpleModelRequest()
            {
                IncludeArchived = filter.IncludeArchived,
                From = filter.From,
                To = filter.To,
                DateType = filter.DateType
            };
            return await Mediator.Send(request);
            
        } 
        [HttpGet("{computerId:guid}")]
        public async Task<ActionResult<GetConcreteComputerResult>> GetConcreteComputer([FromRoute] Guid computerId)
        {
            var request = new GetConcreteComputerRequest()
            {
                ComputerStateId = computerId
            };

            return await Mediator.Send(request);
        }
    }
}
