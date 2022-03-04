using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityAccrual.ClientLibrary.DataAccess;
using UtilityAccrual.Shared.Definitions;
using UtilityAccrual.Shared.Models;

namespace Accounting.Web.Client.Pages.UtilityAccruals.BudgetComponents
{
    public partial class ModifyBudget
    {
        [Inject]
        private ApiHelper _api { get; set; }

        IEnumerable<Utility> utilities = Array.Empty<Utility>();
        Utility selectedUtility;
        Month selectedMonth;
        int selectedYear = DateTime.Now.Year;
        IEnumerable<int> years = Array.Empty<int>();
        IEnumerable<Budget> budgets = Array.Empty<Budget>();
        decimal amount;
        

        protected override async Task OnInitializedAsync()
        {
            utilities = await _api.GetUtilities();
            years = await _api.GetBudgetYears();
            budgets = await _api.GetBudgetsByYear(selectedYear);
        }

        async Task SelectedYearChanged()
        {
            budgets = await _api.GetBudgetsByYear(selectedYear);
        }

        void UpdateAmount()
        {
            if (selectedUtility is null)
                return;
            if (!budgets.Any())
                return;
            var budget = budgets.FirstOrDefault(b => b.Redacted == Redacted.Redacted);
            amount = selectedMonth switch
            {
                // redacted
            };
        }
    }
}
