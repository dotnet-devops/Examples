using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityAccrual.DataAccess.Base;
using UtilityAccrual.Shared.Models;
using UtilityAccrual.Shared.Definitions;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Accounting.Web.Server.Controllers.UtilityAccrualControllers
{
    //! Abandoning this controller until it's necessary to have an approval chain. At the moment, there isn't.

    [Authorize(Roles = "UtilityAccrualUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class ModerationController : ControllerBase
    {
        private readonly SqlClient _sql;

        public ModerationController(SqlClient sql)
        {
            _sql = sql;
        }

        #region Get
        [HttpGet]
        public async Task<IEnumerable<ModerationModel>> Get()
        {
            return await _sql.GetModerations();
        }

        [HttpGet("{id}")]
        public async Task<IEnumerable<ModerationModel>> Get(int id)
        {
            return await _sql.GetModerationsByRevision(id);
        }

        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<ModerationModel> GetById(int id)
        {
            return await _sql.GetModeration(id);
        }

        [Route("[action]/{status}")]
        [HttpGet]
        public async Task<IEnumerable<ModerationModel>> GetByStatus(int status)
        {
            return await _sql.GetModerationsByStatus(status);
        }
        #endregion

        #region Post
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ModerationModel moderation)
        {
            var requests = await _sql.GetModerationsByRevision(moderation.Redacted);
            if (requests.Any(r => r.Redacted is RevisionStatus.Redacted or RevisionStatus.Redacted))
                return BadRequest("This revision has an active request.");
            
            try
            {
                await _sql.Insert(moderation);
                return Ok();
            }
            catch
            {
                return BadRequest("The server failed to initialize a new revision.");
            }
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> Moderate([FromBody] ModerationModel moderation)
        {
            var requests = await _sql.GetModerationsByRevision(moderation.Revision);
            if (requests.Any(r => r.Decision is RevisionStatus.Redacted or RevisionStatus.Redacted))
                return BadRequest("This revision has an active request.");

            try
            {
                await _sql.UpdateModeration(moderation);
                return Ok();
            }
            catch
            {
                return BadRequest("The server failed to initialize a new revision.");
            }
        }

        #endregion
    }
}
