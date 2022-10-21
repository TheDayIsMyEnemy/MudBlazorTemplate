namespace MudBlazorTemplate.Features.Roles
{
    public class IndexBase : ComponentBase
    {
        [Inject]
        private ISnackbar _snackbar { get; set; } = null!;

        [Inject]
        private IDialogService _dialogService { get; set; } = null!;

        [Inject]
        private RoleManager<IdentityRole> _roleManager { get; set; } = null!;

        protected bool IsLoading { get; set; }
        protected List<IdentityRole> Roles { get; set; } = new();

        protected async override Task OnInitializedAsync()
        {
            IsLoading = true;
            Roles = await _roleManager.Roles.ToListAsync();
            IsLoading = false;
        }

        protected async Task CreateRole()
        {
            var result = await _dialogService.Show<CreateRole>("Create Role").Result;

            if (!result.Cancelled)
            {
                var roleName = (string)result.Data;
                var role = new IdentityRole(roleName);

                var identityResult = await _roleManager.CreateAsync(role);

                if (identityResult.Succeeded)
                {
                    _snackbar.Add(string.Format(Messages.SuccessfulCreationFormat, roleName),
                        Severity.Success);
                    Roles.Add(role);
                }
                else
                {
                    _snackbar.Add(string.Join(Environment.NewLine,
                        identityResult.Errors.Select(e => e.Description)), Severity.Error);
                }
            }
        }

        protected async Task DeleteRole(IdentityRole role)
        {
            var parameters = new DialogParameters();
            parameters.Add("Title", "Delete Role");
            parameters.Add("Content", $"Are you really sure you want to delete \"{role}\"?");

            var result = await _dialogService.Show<DeleteConfirmation>("", parameters).Result;

            if (!result.Cancelled)
            {
                var identityResult = await _roleManager.DeleteAsync(role);
                if (identityResult.Succeeded)
                {
                    _snackbar.Add(
                        string.Format(Messages.SuccessfulDeletionFormat, role.Name),
                        Severity.Success);
                    Roles.Remove(role);
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

