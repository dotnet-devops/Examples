using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Threading.Tasks;
using UtilityAccrual.ClientLibrary.DataAccess;
using UtilityAccrual.Shared.Definitions;
using UtilityAccrual.Shared.Models;
using UtilityAccrual.Shared.Models.Display;

namespace Accounting.Web.Client.Pages.UtilityAccruals.Subs
{
    public partial class NewAdjustment
    {
        [Inject]
        private ApiHelper _api{ get; set; }

        [Inject]
        private AuthenticationStateProvider _auth { get; set; }

        [Parameter]
        public AdjustmentDisplayModel AdjustmentModel { get; set; }

        [Parameter]
        public int SelectedMonth { get; set; }

        [Parameter]
        public int SelectedYear { get; set; } 

        [Parameter]
        public EventCallback<CloseCondition> Closed { get; set; }

        private AdjustmentRevision revision = new();
        private List<string> errors = new();
        string username;
        private bool expand;

        public async Task Cancel()
        {
            errors.Clear();
            revision = new();
            username = string.Empty;
            await Closed.InvokeAsync(CloseCondition.Cancelled);
        }

        private async Task InvalidUser()
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                try { await Closed.InvokeAsync(CloseCondition.InvalidUser); }
                catch (Exception) { }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var user = await _auth.GetAuthenticationStateAsync();
            if (user is null)
                await InvalidUser();
            else if (user.User is null)
                await InvalidUser();
            else if (user.User.Identity is null)
                await InvalidUser();
            else if (string.IsNullOrWhiteSpace(user.User.Identity.Name))
                await InvalidUser();
            else username = user.User.Identity.Name;
        }
        protected override void OnParametersSet()
        {
            revision = new();
            errors = new();
            revision.Redacted = AdjustmentModel.Redacted.Redacted;
            revision.Redacted = AdjustmentModel.Redacted.Redacted;
            revision.Redacted = Redacted;
            revision.Redacted = AdjustmentModel.Redacted.Redacted;
            revision.Redacted = AdjustmentModel.Redacted.Redacted;
            revision.Redacted = RevisionStatus.Redacted;
        }

        public string PercentAdjusted
        {
            get
            {
                if (AdjustmentModel == null || revision == null || AdjustmentModel?.Redacted?.Redacted == 0 || revision?.Redacted == 0)
                    return "0%";

                var b = AdjustmentModel.Redacted.Redacted switch
                {
                    // Redacted
                };

                if (revision.Redacted is 0 || b is 0)
                    return "0%";
                return ((revision.Redacted - b) / Math.Abs(b)).ToString("P2");
            }
        }

        private async Task Submit()
        {
            errors.Clear();

            if (revision.Redacted <= 0)
                errors.Add("Redacted must be larger than 0.");
            if (string.IsNullOrWhiteSpace(revision.Redacted))
                errors.Add("A Redacted must be provided.");

            if (errors.Any())
            {
                expand = true;
                return;
            }
                

            var response = await _api.InsertAdjustmentRevision(revision, SelectedMonth, SelectedYear);
            if (response.IsSuccessStatusCode)
            {
                revision = new();
                await Closed.InvokeAsync(CloseCondition.Success);
            }
            else
            {
                expand = true;
                errors.Add(await response.Content.ReadAsStringAsync());
            }
        }
        

    }
}
