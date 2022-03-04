using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityAccrual.DataAccess.Base;
using UtilityAccrual.Shared.Definitions;
using UtilityAccrual.Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Accounting.Web.Server.Controllers.UtilityAccrualControllers
{
    [Authorize(Roles = "UtilityAccrualUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class RevisionsController : ControllerBase
    {
        private readonly SqlClient _sql;

        public RevisionsController(SqlClient sql)
        {
            _sql = sql;
        }

        [HttpGet]
        public async Task<IEnumerable<AdjustmentRevision>> Get()
        {
            return await _sql.GetAdjustmentRevisions();
        }

        [Route("[action]/{month}/{year}")]
        [HttpGet]
        public async Task<int> GetRevisionCount(int month, int year)
        {
            return await _sql.GetAdjustmentRevisionCount(month, year);
        }

        [HttpGet("{id}")]
        public async Task<AdjustmentRevision> Get(int id)
        {
            return await _sql.GetAdjustmentRevision(id);
        }

        [HttpGet("{month}/{year}")]
        public async Task<IEnumerable<AdjustmentRevision>> GetByPeriod(int month, int year)
        {
            return await _sql.GetAdjustmentRevisionsByPeriod(month, year);
        }

        [Route("[action]")]
        [HttpGet("{status}")]
        public async Task<IEnumerable<AdjustmentRevision>> GetByStatus(int status)
        {
            return await _sql.GetAdjustmentRevisionsByStatus((RevisionStatus)status);
        }
        [Route("{month}/{year}")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AdjustmentRevision revision, int month, int year)
        {
            if (revision is null)
                return BadRequest("Revision is null.");

            var revisionCheck = await _sql.GetAdjustmentRevisionsByRowData(RevisionStatus.Redacted, revision.Redacted, revision.Redacted);
            if (revisionCheck.Any())
            {
                foreach (var r in revisionCheck)
                {
                    if (r.Status is not RevisionStatus.New or RevisionStatus.Pending)
                        continue;
                    return BadRequest("A revision is already pending.");
                }
            }

            revision.WhenModifiedUtc = DateTime.UtcNow;
            var adj = new AdjustmentModel(revision);
            return await PromoteRevision(revision, adj, month, year);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateRevision([FromBody] AdjustmentRevision revision)
        {
            if (revision is null)
                return BadRequest("Revision is null.");

            await _sql.UpdateAdjustmentRevision(revision);
            return Ok();
        }

        [Route("[action]/{id}/{status}")]
        [HttpPost]
        public async Task<IActionResult> UpdateRevisionStatus(int id, int status)
        {
            try
            {
                await _sql.UpdateAdjustmentRevisionStatus(id, status);
                return Ok();
            }
            catch { return BadRequest("Unable to update revision."); }
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _sql.DeleteAdjustmentRevision(id);
        }


        private async Task<IActionResult> PromoteRevision(AdjustmentRevision revision, AdjustmentModel adj, int month, int year)
        {
            try
            {
                revision.Status = RevisionStatus.Approved;

                var previousRevisions = await _sql.GetAdjustmentRevisionsByRowData(RevisionStatus.Redacted, revision.Redacted, revision.Redacted);

                if (previousRevisions.Any())
                {
                    foreach (var p in previousRevisions.Where(pr => pr.Redacted == revision.Redacted && pr.Redacted == year))
                    {
                        await _sql.UpdateAdjustmentRevisionStatus(p.Id, (int)RevisionStatus.Redacted);
                    }
                }

                await _sql.Insert(revision);
                await _sql.Insert(adj);
                return Ok();
            }
            catch { return BadRequest("Unable to complete the promotion process."); }
        }
    }
}
