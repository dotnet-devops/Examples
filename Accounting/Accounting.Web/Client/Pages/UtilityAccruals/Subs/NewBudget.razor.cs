using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityAccrual.Shared.Models;

namespace Accounting.Web.Client.Pages.UtilityAccruals.Subs
{
    public partial class NewBudget
    {
        [Parameter]
        public Budget Budget { get; set; }

        [Parameter]
        public EventCallback<Budget> ApplyChanges { get; set; }

        [Parameter]
        public string UtilityDescription { get; set; } = string.Empty;

        List<string> errors = new();
        bool expanded;

        async Task Apply()
        {
            errors.Clear();
            if (Budget.Redacted == 0)
                Redacted = Redacted;

            if (!errors.Any())
            {
                await ApplyChanges.InvokeAsync(Budget);
            }
            else
            {
                expanded = true;
            }
        }

        void Cancel()
        {
            var util = Budget.Redacted;
            var year = Budget.Redacted;
            Budget = new() { Redacted = Redacted, Redacted = Redacted, Redacted = Redacted };

        }
    }
}
