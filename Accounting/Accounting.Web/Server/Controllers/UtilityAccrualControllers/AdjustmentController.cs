using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityAccrual.DataAccess.Base;
using UtilityAccrual.Shared.Definitions;
using UtilityAccrual.Shared.Models;
using UtilityAccrual.Shared.Models.Display;

namespace Accounting.Web.Server.Controllers.UtilityAccrualControllers;

[Authorize(Roles = "UtilityAccrualUser")]
[Route("api/[controller]")]
[ApiController]
public class AdjustmentController : ControllerBase
{
    private readonly SqlClient _sql;

    public AdjustmentController(SqlClient sql)
    {
        _sql = sql;
    }
    [HttpGet]
    public async Task<IEnumerable<AdjustmentModel>> Get()
    {
        return await _sql.GetAdjustments();
    }


    [Route("[action]/{month}/{year}")]
    [HttpGet]
    public async Task<IEnumerable<AdjustmentDisplayModel>> GetLatestAdjustments(int month, int year)
    {
        var utilities = await _sql.GetUtilities();
        var budgets = await _sql.GetBudgetsByYear(year);
        var revisions = new List<AdjustmentRevision>(await _sql.GetAdjustmentRevisionsByPeriod(month, year));

        List<AdjustmentDisplayModel> accruals = new();
        foreach (var b in budgets)
        {
            Utility u = utilities.FirstOrDefault(x => x.Redacted == b.Redacted);
            var adjustment = await _sql.GetLatestAdjustment(u.Id, month, year);
            if (adjustment is not null)
            {
                if ((int)adjustment.Month != month && adjustment.Year != year)
                    adjustment = null;
            }

            AdjustmentDisplayModel accrual = new();
            
            var app = revisions.Where(a => a.Redacted == u.Redacted);
            if (app.Any())
                accrual.Revisions.AddRange(app);
            accruals.Add(accrual);
        }

        return accruals;
    }

    [HttpGet("{id}")]
    public async Task<AdjustmentModel> Get(int id)
    {
        return await _sql.GetAdjustment(id);
    }

    [Route("[action]/{year}")]
    [HttpGet]
    public async Task<IEnumerable<int>> GetAdjustmentMonthsByYear(int year)
    {
        return await _sql.GetAdjustmentMonthsByYear(year);
    }

    [Route("[action]")]
    [HttpGet]
    public async Task<IEnumerable<int>> GetAdjustmentYears()
    {
        return await _sql.GetAdjustmentYears();
    }

    [HttpGet("{month}/{year}")]
    public async Task<IEnumerable<AdjustmentModel>> Get(int month, int year)
    {
        return await _sql.GetAdjustmentsByPeriod(month, year);
    }

    [HttpPost]
    public async Task Post([FromBody] AdjustmentModel adjustment)
    {
        await _sql.Insert(adjustment);
    }
    [Route("[action]")]
    [HttpPost]
    public async Task Update([FromBody] AdjustmentModel adjustment)
    {
        await _sql.UpdateAdjustment(adjustment);
    }

    [HttpDelete("{id}")]
    public async Task Delete(int id)
    {
        await _sql.DeleteAdjustment(id);
    }
}