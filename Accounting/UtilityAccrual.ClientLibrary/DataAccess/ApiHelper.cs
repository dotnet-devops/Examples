using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using UtilityAccrual.Shared.Models;
using UtilityAccrual.Shared.Models.Display;

namespace UtilityAccrual.ClientLibrary.DataAccess
{
    public class ApiHelper
    {
        private HttpClient _http;

        public ApiHelper() { }
        public ApiHelper(HttpClient http)
        {
            _http = http;
        }

        public void Set(HttpClient http)
        {
            _http = http;
        }

        #region Budget

        public async Task<IEnumerable<Budget>> GetBudgetsByYear(int year)
        {
            return await _http.GetFromJsonAsync<IEnumerable<Budget>>($"api/budget/GetByYear/{year}");
        }

        public async Task<IEnumerable<BudgetRevision>> GetBudgetRevisionsByYear(int year)
        {
            return await _http.GetFromJsonAsync<IEnumerable<BudgetRevision>>($"api/budget/GetRevisionsByYear/{year}");
        }

        public async Task<IEnumerable<BudgetRevision>> GetBudgetRevisionsByBudget(int budget)
        {
            return await _http.GetFromJsonAsync<IEnumerable<BudgetRevision>>($"api/budget/GetRevisionsByBudget/{budget}");
        }

        public async Task<IEnumerable<int>> GetBudgetYears()
        {
            return await _http.GetFromJsonAsync<IEnumerable<int>>("api/budget/GetBudgetYears");
        }

        public async Task<HttpResponseMessage> NewBudget(Budget budget)
        {
            return await _http.PostAsJsonAsync("api/budget", budget);
        }


        #endregion

        #region Adjustments

        public async Task<IEnumerable<AdjustmentModel>> GetAdjustments()
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdjustmentModel>>("api/adjustment");
        }

        public async Task<List<AdjustmentDisplayModel>> GetLatestAdjustments(int month, int year)
        {
            return await _http.GetFromJsonAsync<List<AdjustmentDisplayModel>>($"api/adjustment/GetLatestAdjustments/{month}/{year}");
        }

        public async Task<int[]> GetAdjustmentMonthsByYear(int year)
        {
            return await _http.GetFromJsonAsync<int[]>("api/adjustment/GetAdjustmentMonthsByYear/" + year);
        }

        public async Task<IEnumerable<AdjustmentModel>> GetAdjustmentsByPeriod(int month, int year)
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdjustmentModel>>($"api/adjustment/{month}/{year}");
        }

        public async Task<int[]> GetAdjustmentYears()
        {
            return await _http.GetFromJsonAsync<int[]>("api/adjustment/GetAdjustmentYears");
        }

        public async Task<HttpResponseMessage> InsertAdjustment(AdjustmentModel adjustment)
        {
            return await _http.PostAsJsonAsync("api/adjustment", adjustment);
        }

        public async Task<HttpResponseMessage> InsertAdjustmentRevision(AdjustmentRevision adjustment, int month, int year)
        {
            return await _http.PostAsJsonAsync($"api/revisions/{ month }/{ year }", adjustment);
        }

        public async Task<HttpResponseMessage> UpdateAdjustment(AdjustmentModel adjustment)
        {
            return await _http.PostAsJsonAsync("api/adjustment/Update", adjustment);
        }

        public async Task<HttpResponseMessage> DeleteAdjustment(int id)
        {
            return await _http.DeleteAsync("api/adjustment/" + id);
        }

        #endregion

        #region Editors

        public async Task<IEnumerable<Editor>> GetEditors()
        {
            return await _http.GetFromJsonAsync<IEnumerable<Editor>>("api/editor");
        }
        #endregion

        #region Utilities

        public async Task<HttpResponseMessage> ArchiveUtility(int id)
        {
            return await _http.PostAsJsonAsync("api/utilities/archive", new { id = id });
        }

        public async Task<HttpResponseMessage> UnarchiveUtility(int id)
        {
            return await _http.PostAsJsonAsync("api/utilities/unarchive", new { id = id });
        }

        public async Task<HttpResponseMessage> InsertUtility(Utility utility)
        {
            return await _http.PostAsJsonAsync("api/utilities", utility);
        }

        public async Task<IEnumerable<Utility>> GetArchivedUtilities()
        {
            return await _http.GetFromJsonAsync<IEnumerable<Utility>>("api/utilities/GetArchivedUtilities");
        }

        public async Task<IEnumerable<Utility>> GetUtilities()
        {
            var utils = await _http.GetFromJsonAsync<IEnumerable<Utility>>("api/utilities");
            return utils;
        }

        public async Task<Utility> GetUtilityById(int id)
        {
            return await _http.GetFromJsonAsync<Utility>("api/utilities/" + id);
        }

        public async Task<HttpResponseMessage> UpdateUtility(Utility utility)
        {
            return await _http.PostAsJsonAsync("api/utilities/update", utility);
        }

        public async Task DeleteUtility(int id)
        {
            await _http.DeleteAsync("api/utilities/" + id);
        }
        #endregion

        #region Revisions

        public async Task<int> GetRevisionCount(int month, int year)
        {
            return await _http.GetFromJsonAsync<int>($"api/revisions/getrevisioncount/{month}/{year}");
        }

        public async Task<IEnumerable<AdjustmentRevision>> GetLastFiftyRevisions()
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdjustmentRevision>>($"api/revisions/GetLastFifty");
        }

        public async Task<IEnumerable<AdjustmentRevision>> GetRevisionsByPeriod(int month, int year)
        {
            return await _http.GetFromJsonAsync<IEnumerable<AdjustmentRevision>>($"api/revisions/{month}/{year}");
        }

        #endregion
    }
}
