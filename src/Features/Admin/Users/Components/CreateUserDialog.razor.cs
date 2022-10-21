namespace MudBlazorTemplate.Features.Admin.Users.Components
{
    public class CreateUserDialogBase : ComponentBase
    {
        protected bool IsLoading { get; set; }
        protected CreateUserFormModel FormModel { get; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        [Parameter]
        public Func<CreateUserFormModel, Task<bool>> CreateUser { get; set; } = null!;

        protected async Task OnValidSubmit()
        {
            IsLoading = true;

            var succeeded = await CreateUser(FormModel);  
            if (succeeded)
                MudDialog.Close();

            IsLoading = false;
        }
    }


    public class CreateUserFormModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }
    }
}
