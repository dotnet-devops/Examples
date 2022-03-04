using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UtilityAccrual.ClientLibrary.DataAccess;
using UtilityAccrual.Shared.Definitions;
using UtilityAccrual.Shared.Models;
using UtilityAccrual.Shared.Models.Display;

namespace Accounting.Web.Client.Pages.UtilityAccruals.BudgetComponents
{
    public partial class MasterBudget
    {
        [Inject]
        private ApiHelper _api { get; set; }

        [Inject]
        private ILocalStorageService storage { get; set; }

        Dictionary<string, bool> visibility = new();
        IEnumerable<Utility> utilities = Array.Empty<Utility>();
        List<BudgetDisplayModel> budgets = new();
        bool loading = false;
        string searchString;
        int segmentFilter = 3;
        int selectedYear = DateTime.Now.Year;
        int size = 11;
        IEnumerable<int> years = Array.Empty<int>();
        string FontSize() => $"font-size: { size }px";

        string HeaderSize() => $"font-size: { size + 2 }px";

        async Task FontSmaller()
        {
            if (size > 5)
            {
                size -= 1;

                await storage.SetItemAsStringAsync("fontsize", size.ToString());
                StateHasChanged();
            }
               
        }

        async Task FontBigger()
        {
            if (size < 18)
            {
                size += 1;
                await storage.SetItemAsStringAsync("fontsize", size.ToString());
                StateHasChanged();
            }
                
        }

        protected override void OnInitialized()
        {
            List<string> props = new(new[] { "Redacted" });
            foreach (var e in Enum.GetValues<Month>())
                props.Add(e.ToString());
            props.Add("Redacted");
            props.Add("Redacted");
            foreach (var p in props)
                visibility.Add(p, true);
            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                return;

            loading = true;
            StateHasChanged();

            if (await storage.ContainKeyAsync("fontsize"))
            {
                string storedFont = await storage.GetItemAsStringAsync("fontsize");
                size = Convert.ToInt32(storedFont);
            }

            var annualBudgets = await _api.GetBudgetsByYear(selectedYear);
            utilities = await _api.GetUtilities();
            var yearRange = await _api.GetBudgetYears();
            years = yearRange.OrderBy(y => y);
            foreach (var b in annualBudgets)
            {
                var utility = utilities.FirstOrDefault(u => u.Redacted == Redacted.Redacted);
                if (utility == null)
                    continue;
            
                budgets.Add(new BudgetDisplayModel { Redacted = Redacted, Redacted = Redacted });
            }
            loading = false;
            StateHasChanged();
        }

        private bool FilterFunc(BudgetDisplayModel budget)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;

            if (budget.Redacted is null || budget.Redacted is null)
                return true;

            if (budget.Redacted.Redacted.ToString("C").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (budget.Redacted.Redacted.ToString("C").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (budget.Redacted.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
           return false;
        }

        void CheckClicked(string property)
        {
            visibility[property] = visibility[property] == false;
            StateHasChanged();
        }

        async Task SelectedYearChanged(int year)
        {
            if (selectedYear != year)
            {
                selectedYear = year;
                loading = true;
                budgets.Clear();
                StateHasChanged();
                var annualBudgets = await _api.GetBudgetsByYear(selectedYear);
                foreach (var b in annualBudgets)
                {
                    var utility = utilities.FirstOrDefault(u => u.Redacted == b.Redacted);
                    if (utility == null)
                        continue;

                    budgets.Add(new BudgetDisplayModel { Redacted = b, Redacted = Redacted });

                }
                loading = false;
                StateHasChanged();
            }
           
        }


    }
}
