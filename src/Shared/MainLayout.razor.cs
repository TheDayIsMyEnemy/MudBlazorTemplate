using Microsoft.AspNetCore.Components;
using MudBlazor;
using MudBlazorTemplate.Theme;

namespace MudBlazorTemplate.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
        protected MudThemeProvider? MudThemeProvider { get; set; } = new();
        protected DefaultTheme? DefaultTheme { get; set; } = new();
        protected bool IsDrawerOpened { get; set; } = true;
        protected bool IsDarkMode { get; set; }

        protected void DrawerToggle()
            => IsDrawerOpened = !IsDrawerOpened;

        protected void DarkModeToggle()
            => IsDarkMode = !IsDarkMode;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                IsDarkMode = await MudThemeProvider!.GetSystemPreference();
                StateHasChanged();
            }
        }
    }
}
