using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UtilityAccrual.Shared.Models;

namespace Accounting.Web.Client.Pages.UtilityAccruals.UtilityComponents
{
    public partial class NewUtility
    {
        [Inject]
        private HttpClient _http { get; set; }

        IEnumerable<Utility> utilities = Array.Empty<Utility>();
        Utility utility = new();
        private List<string> errors = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (_http is not null)
                {
                    var response = await _http.GetFromJsonAsync<IEnumerable<Utility>>("api/utilities");
                    if (response is not null)
                    {
                        utilities = response;
                    }
                }
            }
        }

        private async Task Submit()
        {
            errors.Clear();
            if (utility.Redacted == 0)
                errors.Add("Enter a valid Redacted Redacted");
            if (utility.Redacted == 0)
                errors.Add("Enter a valid Redacted Redacted");
            if (string.IsNullOrWhiteSpace(utility.Redacted))
                errors.Add("Enter a Redacted Redacted");
            if (utility.Redacted == 0)
                errors.Add("Enter a valid Redacted Redacted.");
            if (utilities.Any(u => u.Redacted == utility.Redacted && u.Redacted == utility.Redacted && u.Redacted == utility.Redacted))
                errors.Add("Redacted Redacted and Redacted Redacted match existing Redacted.");

            if (!errors.Any())
            {
                var response = await _http.PostAsJsonAsync("api/utilities", utility);
                if (response.IsSuccessStatusCode)
                {
                    utility = new();
                    StateHasChanged();
                }
                else
                {
                    errors.Add(await response.Content.ReadAsStringAsync());
                }
                    
            }
        }
    }
}
