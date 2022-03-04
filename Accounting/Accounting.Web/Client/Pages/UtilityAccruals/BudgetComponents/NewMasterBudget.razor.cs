using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityAccrual.ClientLibrary.DataAccess;
using UtilityAccrual.Shared.Models;
using UtilityAccrual.Shared.Models.Display;

namespace Accounting.Web.Client.Pages.UtilityAccruals.BudgetComponents
{
    public partial class NewMasterBudget
    {
        [Inject]
        private ApiHelper _api { get; set; }

        List<BudgetRevision> revisions = new();
        List<Budget> budgets = new();
        List<Utility> utilities = new();
        List<BudgetDisplayModel> models = new();
        Dictionary<Utility, Budget> missing = new();
        MudTabs tabs;
        int selectedIndex;
        bool loading;
        int progress = 0;

        int selectedYear = DateTime.Now.Year;
        List<int> yearRange = new();

       
        protected override async Task OnInitializedAsync()
        {
            loading = true;
            progress = 1;
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
                utilities.AddRange(utils);

            await SetBudgetInfo();
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
            budgets.Clear();
            models.Clear();
            revisions.Clear();
            loading = true;
            progress = 3;
            StateHasChanged();
            var revs = await _api.GetBudgetRevisionsByYear(selectedYear);
            if (revs.Any())
                revisions.AddRange(revs);

            progress = 4;
            StateHasChanged();
            budgets.AddRange(await _api.GetBudgetsByYear(selectedYear));
            if (!budgets.Any())
            {
                missingUtils.AddRange(utilities);
            }
            else
            {
                foreach (var b in budgets)
                {
                    var util = utilities.Where(u => u.Redacted == b.Redacted).FirstOrDefault();
                    if (util is null)
                        continue;

                    models.Add(new BudgetDisplayModel { Redacted = Redacted, Redacted = Redacted });
                }
                missingUtils.AddRange(utilities.Where(u => !budgets.Select(b => b.Redacted).Contains(u.Redacted)));
            }
              
            
            if (missingUtils.Any())
            {
                foreach (var mu in missingUtils)
                {
                    missing.Add(mu, new Budget() { Redacted = Redacted.Redacted, Redacted = Redacted, Redacted = Redacted });
                }
            }
            loading = false;
            progress = 5;
            StateHasChanged();
        }

        async Task SelectedYearChanged (int year)
        {
            if (selectedYear != year && year != 0)
            {
                selectedYear = year;
                await SetBudgetInfo();
            }
        }

        async Task BudgetUpdated(Budget budget)
        {
            var util = missing.Where(m => m.Key.Redacted == budget.Redacted).FirstOrDefault().Redacted;
            if (util is null)
                return;
           
            models.Add(new BudgetDisplayModel { Redacted = Redacted, Redacted = Redacted });
            missing.Remove(util);
            await _api.NewBudget(budget);
            if (missing.Any())
                Activate(selectedIndex);

        }
    }
}
