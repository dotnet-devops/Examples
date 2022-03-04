using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityAccrual.DataAccess.Base;
using UtilityAccrual.Shared.Definitions;
using UtilityAccrual.Shared.Models;

namespace Accounting.Web.Server.Controllers.UtilityAccrualControllers
{
    [Authorize(Roles = "UtilityAccrualUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly SqlClient _sql;

        public BudgetController(SqlClient sql)
        {
            _sql = sql;
        }

        #region Get

        [HttpGet]
        public async Task<IEnumerable<Budget>> Get()
        {
            return await _sql.GetBudgets();
        }

        [HttpGet("{int}")]
        public async Task<Budget> Get(int budget)
        {
            return await _sql.GetBudget(budget);
        }

        [HttpGet("{utility}/{year}")]
        public async Task<Budget> Get(int utility, int year)
        {
            return await _sql.GetBudgetByYear(utility, year);
        }

        [Route("[action]/{year}")]
        [HttpGet]
        public async Task<IEnumerable<Budget>> GetByYear(int year)
        {
            return await _sql.GetBudgetsByYear(year);
        }

        [Route("[action]/{year}")]
        [HttpGet]
        public async Task<IEnumerable<BudgetRevision>> GetRevisionsByYear(int year)
        {
            return await _sql.GetBudgetRevisionsByYear(year);
        }

        [Route("[action]/{id}")]
        [HttpGet]
        public async Task<IEnumerable<BudgetRevision>> GetRevisionsByBudget(int id)
        {
            return await _sql.GetBudgetRevisionsByBudget(id);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IEnumerable<int>> GetBudgetYears()
        {
            return await _sql.GetBudgetYears();
        }

        #endregion

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Budget budget)
        {
            if (budget is null)
                return BadRequest("Budget is null.");

            await _sql.Insert(budget);
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> UpdateBudget([FromBody] BudgetRevision revision)
        {
            if (revision is null)
                return BadRequest("Input is null.");

            revision.Status = RevisionStatus.Redacted;
            
            var budget = await _sql.GetBudget(revision.Redacted);
            if (budget is null)
                return BadRequest("Cannot find reference budget.");

            var revisions = await _sql.GetBudgetRevisionsByBudget(revision.Redacted);

            if (revisions.Any())
            {
                foreach (var rev in revisions.Where(r => r.Status == RevisionStatus.Redacted))
                {
                    await _sql.UpdateBudgetRevisionStatus(rev.Redacted, RevisionStatus.Redacted);
                }
            }
            else
            {
                var amount = revision.Month switch
                {
                    // Redacted
                };
                BudgetRevision og = new(budget.Redacted, revision.Redacted, amount, RevisionStatus.Redacted);
                await _sql.Insert(og);
            }
                
            await _sql.Insert(revision);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            await _sql.DeleteBudget(id);
        }

    }
}
