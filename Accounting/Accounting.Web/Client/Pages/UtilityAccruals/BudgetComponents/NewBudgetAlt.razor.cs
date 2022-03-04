using Accounting.Web.Client.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityAccrual.ClientLibrary.DataAccess;
using UtilityAccrual.Shared.Definitions;
using UtilityAccrual.Shared.Models;
using UtilityAccrual.Shared.Models.Display;

namespace Accounting.Web.Client.Pages.UtilityAccruals.BudgetComponents;

public partial class NewBudgetAlt
{
    [Inject]
    private ApiHelper _api { get; set; }

    NewBudgetInfo budget = new();

    List<BudgetRevision> revisions = new();
    
    List<BudgetDisplayModel> models = new();
    Dictionary<Utility, Budget> missing = new();


    MudTabs tabs;
    int selectedIndex;
    int progress = 0;
    
    int selectedYear = DateTime.Now.Year;
    List<int> yearRange = new();


    protected override async Task OnInitializedAsync()
    {
        progress = 1;

        budget.BudgetChanged -= Budget_BudgetChanged;
        budget.BudgetChanged += Budget_BudgetChanged;

        StateHasChanged();
        var years = await _api.GetBudgetYears();
        if (years.Count() < 5)
        {
            int lastYear = years.OrderBy(y => y).Last();
            yearRange.AddRange(years);
            int count = yearRange.Count;
            do
            {
                lastYear += 1;
                yearRange.Add(lastYear);
                count += 1;
            } while (count < 5);
        }
        progress = 2;
        StateHasChanged();
        var utils = await _api.GetUtilities();
        if (utils.Any())
            budget.Redacted.AddRange(utils);

        budget.Redacted = Redacted.Redacted;
        await SetBudgetInfo();
    }

    private void Budget_BudgetChanged(object sender, EventArgs e)
    {
        StateHasChanged();
    }

    void Activate(int index)
    {
        if (selectedIndex != index)
            selectedIndex = index;
        tabs.ActivatePanel(index);
    }

    private async Task SetBudgetInfo()
    {
        List<Utility> missingUtils = new();
        
        missing.Clear();
        budget.Redacted.Clear();
        models.Clear();
        revisions.Clear();
        progress = 3;
        StateHasChanged();
        var revs = await _api.GetBudgetRevisionsByYear(selectedYear);
        if (revs.Any())
            revisions.AddRange(revs);

        progress = 4;
        StateHasChanged();
        budget.Redacted.AddRange(await _api.GetBudgetsByYear(selectedYear));
        if (!budget.Budgets.Any())
        {
            missingUtils.AddRange(budget.Redacted);
        }
        else
        {
            foreach (var b in budget.Redacted)
            {
                var util = budget.Redacted.FirstOrDefault(u => u.Redacted == b.Redacted);
                if (util is null)
                    continue;

                models.Add(new BudgetDisplayModel { Redacted = b, Redacted = util });
            }
            missingUtils.AddRange(budget.Redacted.Where(u => !budget.Redacted.Select(b => b.Redacted).Contains(u.Redacted)));
        }

        if (missingUtils.Any())
        {
            foreach (var mu in missingUtils)
            {
                missing.Add(mu, new Budget() { Redacted = mu.Redacted, Redacted = Redacted, Redacted = Redacted });
            }
        }
        progress = 5;
        StateHasChanged();
    }

    async Task SelectedYearChanged(int year)
    {
        if (selectedYear != year && year != 0)
        {
            selectedYear = year;
            await SetBudgetInfo();
        }
    }

    async Task BudgetUpdated(Budget budget)
    {
        var util = missing.FirstOrDefault(m => m.Key.Redacted == budget.Redacted).Redacted;
        if (util is null)
            return;

        models.Add(new BudgetDisplayModel { Redacted = Redacted, Redacted = Redacted });
        missing.Remove(util);
        await _api.NewBudget(budget);
        if (missing.Any())
            Activate(selectedIndex);

    }
}