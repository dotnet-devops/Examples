using BlazorClientLibrary.Components.Helpers;
using Microsoft.AspNetCore.Components;

namespace ApiDemo.Web.Client.Shared
{
    public partial class MainLayout
    {
        [Inject]
        private ThemeHelper Theme { get; set; }

        [Inject]
        private TableHelper Table { get; set; }

        protected override async Task OnInitializedAsync()
        {
            string[] props = { "None" };
            Table.SetVisibilityTokens(props);
            await Table.RecallLocalVariables();
            await Theme.GetTheme();
        }
    }
}
