using MudBlazorTemplate.Features.Admin.Users.Components;

namespace MudBlazorTemplate.Features.Admin.Users
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        private ISnackbar _snackbar { get; set; } = null!;
        [Inject]
        protected IDialogService _dialogService { get; set; } = null!;
        [Inject]
        private UserManager<User> _userManager { get; set; } = null!;
        [CascadingParameter]
        private Task<AuthenticationState> _authStateTask { get; set; } = null!;

        protected bool IsLoading { get; set; }
        protected string? SearchQuery { get; set; }
        protected string CurrentUserId { get; set; } = null!;
        protected List<User> Users { get; set; } = new();

        private async Task LoadUsers()
        {
            IsLoading = true;
            Users = await _userManager.Users.ToListAsync();
            var authState = await _authStateTask;
            CurrentUserId = authState.User.GetId()!;
            IsLoading = false;
        }

        protected override async Task OnInitializedAsync()
            => await LoadUsers();

        protected bool FilterUsers(User user)
        {
            if (string.IsNullOrEmpty(SearchQuery))
                return true;
            else if (user.Email.Includes(SearchQuery))
                return true;
            else if (user.FirstName.Includes(SearchQuery))
                return true;
            else if (user.LastName.Includes(SearchQuery))
                return true;

            return false;
        }

        protected async Task OpenCreateUserDialog()
        {
            var parameters = new DialogParameters();
            parameters.Add("CreateUser", new Func<CreateUserFormModel, Task<bool>>(CreateUser));
            await _dialogService.Show<CreateUserDialog>("Create user", parameters).Result;
        }

        private async Task<bool> CreateUser(CreateUserFormModel userData)
        {
            var user = new User
            {
                FirstName = userData.FirstName,
                LastName = userData.LastName,
                Email = userData.Email,
                UserName = userData.Email
            };

            var result = await _userManager.CreateAsync(user, userData.Password);

            var message = string.Format(Messages.SuccessfulCreationFormat, user.Email);
            var severity = Severity.Success;

            if (result.Succeeded)
            {
                Users.Add(user);
            }
            else
            {
                message = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
                severity = Severity.Error;
            }

            _snackbar.Add(message, severity);
            return result.Succeeded;
        }

        //protected async Task ShowAssignRolesDialog(User user)
        //{
        //    //var currentRoles = await _userManager.GetRolesAsync(user);

        //    //var parameters = new DialogParameters();
        //    //parameters.Add("UserRoles", currentRoles);
        //    //parameters.Add("SelectMultipleRoles", true);

        //    //var result = await _dialogService
        //    //    .Show<AssignRolesDialog>("Assign Roles", parameters).Result;
        //}

        protected async Task AssignRoles(User user)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);

            var parameters = new DialogParameters();
            parameters.Add("UserRoles", currentRoles);
            parameters.Add("SelectMultipleRoles", true);

            var result = await _dialogService
                .Show<AssignRolesDialog>("Assign Roles", parameters).Result;

            if (!result.Cancelled)
            {
                IdentityResult? identityResult = null;
                var newRoles = (IEnumerable<string>)result.Data;
                var rolesToAdd = newRoles.Except(currentRoles).ToList();
                var rolesToRemove = currentRoles.Except(newRoles).ToList();

                if (rolesToRemove.Any())
                {
                    identityResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                    if (!identityResult.Succeeded)
                    {
                        _snackbar.Add(string.Join(Environment.NewLine,
                            identityResult.Errors.Select(e => e.Description)), Severity.Error);
                        return;
                    }
                }
                if (rolesToAdd.Any())
                {
                    identityResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                    if (!identityResult.Succeeded)
                    {
                        _snackbar.Add(string.Join(Environment.NewLine,
                            identityResult.Errors.Select(e => e.Description)), Severity.Error);
                        return;
                    }
                }
                if (identityResult != null)
                {
                    _snackbar.Add(string.Format(Messages.SuccessfulUpdateFormat, user.Email),
                        Severity.Success);
                }
            }
        }

        protected async Task BlockOrUnblockUser(User user)
        {
            var action = user.IsBlocked ? "unblock" : "block";

            user.IsBlocked = !user.IsBlocked;
            var identityResult = await _userManager.UpdateAsync(user);

            if (identityResult.Succeeded)
            {
                _snackbar.Add($"User {user.Email} has been {action}ed.", Severity.Success);
            }
            else
            {
                _snackbar.Add(string.Join(Environment.NewLine,
                    identityResult.Errors.Select(e => e.Description)), Severity.Error);
            }
        }

        protected async Task DeleteUser(User user)
        {
            var parameters = new DialogParameters();
            parameters.Add("Title", "Delete user");
            parameters.Add("Content", $"Are you sure you want to delete \"{user.Email}\"?");

            var result = await _dialogService.Show<DeleteConfirmation>("", parameters).Result;

            if (!result.Cancelled)
            {
                var identityResult = await _userManager.DeleteAsync(user);
                if (identityResult.Succeeded)
                {
                    _snackbar.Add(
                        string.Format(Messages.SuccessfulDeletionFormat, user.Email),
                        Severity.Success);
                    Users.Remove(user);
                }
                else
                {
                    _snackbar.Add(string.Join(Environment.NewLine,
                        identityResult.Errors.Select(e => e.Description)), Severity.Error);
                }
            }
        }
    }
}
