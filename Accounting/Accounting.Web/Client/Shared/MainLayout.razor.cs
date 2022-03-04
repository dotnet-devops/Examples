using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using UtilityAccrual.ClientLibrary.DataAccess;
using UtilityAccrual.ClientLibrary.Helpers;

namespace Accounting.Web.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private ApiHelper _api { get; set; }

        [Inject]
        private HttpClient _http { get; set; }

        [Inject]
        private TableHelper _table { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await _table.RecallLocalVariables();
            _api.Set(_http);
            string preference = string.Empty;
            if (await localStorage.ContainKeyAsync("theme"))
            {
                preference = await localStorage.GetItemAsStringAsync("theme");
                if (preference == "light")
                    currentTheme = theme;
                else if (preference == "dark")
                    currentTheme = darkTheme;
            }
            else
            {
                currentTheme = theme;
            }
        }
    }
}
