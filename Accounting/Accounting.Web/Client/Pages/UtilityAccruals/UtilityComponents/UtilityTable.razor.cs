using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityAccrual.ClientLibrary.DataAccess;
using UtilityAccrual.ClientLibrary.Helpers;
using UtilityAccrual.Shared.Models;

namespace Accounting.Web.Client.Pages.UtilityAccruals.UtilityComponents
{
    public partial class UtilityTable
    {
        [Inject]
        private ApiHelper _api { get; set; }

        [Inject]
        private TableHelper _table { get; set; }

        private List<Utility> utilities = new();
        private List<Utility> archived = new();
        private bool loading = false;
        private string searchString;
        private int segmentFilter = 3;
        private int taskProgress = 0;
        private string task = string.Empty;
        private int _selectedUtility = 0;


        public int SelectedUtility
        {
            get { return _selectedUtility; }
            set 
            {
                if (_selectedUtility == value)
                    _selectedUtility = 0;
                else
                    _selectedUtility = value;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender)
                return;

            loading = true;
            task = "Getting Utilities...";
            StateHasChanged();

            var utils = await _api.GetUtilities();
            if (utils.Any())
                utilities.AddRange(utils);
            task = "Getting Archived Utilities...";
            taskProgress = 1;
            StateHasChanged();

            var archivedUtilities = await _api.GetArchivedUtilities();
            if (archivedUtilities.Any())
                archived.AddRange(archivedUtilities);
            taskProgress = 2;
            loading = false;
            task = "Done!";
            StateHasChanged();
            await Task.Delay(750);
            loading = false;
            StateHasChanged();
        }

        private bool FilterFunc(Utility util)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;

            if (util.Redacted.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (util.Redacted.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (util.Redacted.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (util.Redacted.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (util.Redacted.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        }


        async Task Archive(int id)
        {
            var response = await _api.ArchiveUtility(id);
            if (response.IsSuccessStatusCode)
            {
                foreach (var u in utilities.ToArray())
                {
                    if (u.Id != id)
                        continue;

                    utilities.Remove(u);
                    archived.Add(u);
                    SelectedUtility = 0;
                    StateHasChanged();
                    break;
                }
            }
        }

        async Task Unarchive(int id)
        {
            var response = await _api.UnarchiveUtility(id);
            if (response.IsSuccessStatusCode)
            {
                foreach (var u in archived.ToArray())
                {
                    if (u.Id != id)
                        continue;

                    utilities.Add(u);
                    archived.Remove(u);
                    SelectedUtility = 0;
                    StateHasChanged();
                    break;
                }
            }
        }
    }
}
