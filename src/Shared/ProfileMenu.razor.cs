using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazorTemplate.Extensions;

namespace MudBlazorTemplate.Shared
{
    public class ProfileMenuBase : ComponentBase
    {
        [CascadingParameter]
        private Task<AuthenticationState> _authStateTask { get; set; } = null!;

        protected string FullName { get; set; } = null!;
        protected string? Role { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await _authStateTask;
            FullName = authState.User.GetFullName()!;
            Role = authState.User.GetRole();
        }
    }
}
