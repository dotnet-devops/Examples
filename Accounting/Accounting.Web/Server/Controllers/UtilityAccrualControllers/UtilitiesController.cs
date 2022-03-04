using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityAccrual.DataAccess.Base;
using UtilityAccrual.Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Accounting.Web.Server.Controllers.UtilityAccrualControllers
{
    [Authorize(Roles = "UtilityAccrualUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class UtilitiesController : ControllerBase
    {
        private readonly SqlClient _sql;

        public UtilitiesController(SqlClient sql)
        {
            _sql = sql;
        }

        // GET: api/<UtilitiesController>
        
        [HttpGet]
        public async Task<IEnumerable<Utility>> Get()
        {
            var utils = await _sql.GetUtilities();
            return utils;
        }

        [HttpGet("{id}")]
        public async Task<Utility> Get(int id)
        {
            return await _sql.GetUtility(id);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IEnumerable<Utility>> GetArchivedUtilities()
        {
            return await _sql.GetArchivedUtilities();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Utility utility)
        {
            if (utility is null)
                return BadRequest("Utility is null.");

            await _sql.Insert(utility);

            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] Utility utility)
        {
            if (utility is null)
                return BadRequest("Utility is null.");

            await _sql.UpdateUtility(utility);

            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Archive([FromBody] int id)
        {
            await _sql.ArchiveUtility(id);

            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Unarchive([FromBody]int id)
        {
            await _sql.UnarchiveUtility(id);

            return Ok();
        }

        // DELETE api/<UtilitiesController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _sql.DeleteUtility(id);
        }
    }
}
