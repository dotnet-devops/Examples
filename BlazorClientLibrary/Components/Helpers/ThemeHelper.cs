using Blazored.LocalStorage;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorClientLibrary.Components.Helpers
{
    public class ThemeHelper
    {
        private readonly ILocalStorageService _local;

        public MudTheme DefaultTheme = new();
        public MudTheme CurrentTheme = new();

        public ThemeHelper(ILocalStorageService local)
        {
            _local = local;
        }

        public MudTheme DarkTheme = new()
        {
            Palette = new Palette()
            {
                Black = "#27272f",
                Background = "#32333d",
                BackgroundGrey = "#27272f",
                Surface = "#373740",
                DrawerBackground = "#27272f",
                DrawerText = "rgba(255,255,255, 0.50)",
                DrawerIcon = "rgba(255,255,255, 0.50)",
                AppbarBackground = "#27272f",
                AppbarText = "rgba(255,255,255, 0.70)",
                TextPrimary = "rgba(255,255,255, 0.70)",
                TextSecondary = "rgba(255,255,255, 0.50)",
                ActionDefault = "#adadb1",
                ActionDisabled = "rgba(255,255,255, 0.26)",
                ActionDisabledBackground = "rgba(255,255,255, 0.12)",
                Divider = "rgba(255,255,255, 0.12)",
                DividerLight = "rgba(255,255,255, 0.06)",
                TableLines = "rgba(255,255,255, 0.12)",
                LinesDefault = "rgba(255,255,255, 0.12)",
                LinesInputs = "rgba(255,255,255, 0.3)",
                TextDisabled = "rgba(255,255,255, 0.2)",
                Primary = "rgba(255,255,255, 0.7)"
            }
        };

        public async Task DarkMode()
        {
            if (CurrentTheme == DefaultTheme)
            {
                await _local.SetItemAsStringAsync("theme", "dark");
                CurrentTheme = DarkTheme;
            }

            else
            {
                await _local.SetItemAsStringAsync("theme", "light");
                CurrentTheme = DefaultTheme;
            }
        }

        public async Task GetTheme()
        {
            if (await _local.ContainKeyAsync("theme"))
            {
                if (await _local.GetItemAsStringAsync("theme") == "light")
                    CurrentTheme = DefaultTheme;
                else CurrentTheme = DarkTheme;
            }
            else
            {
                CurrentTheme = DefaultTheme;
            }
        }
    }
}
