using Microsoft.AspNetCore.Identity;
using MudBlazorTemplate.Data.Entities;

namespace MudBlazorTemplate.Areas.Identity
{
    public class UserConfirmation : IUserConfirmation<User>
    {
        public Task<bool> IsConfirmedAsync(UserManager<User> manager, User user)
            => Task.FromResult(!user.IsBlocked);
    }
}
