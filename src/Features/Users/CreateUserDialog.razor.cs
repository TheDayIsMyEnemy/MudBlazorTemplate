using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace MudBlazorTemplate.Features.Users
{
    public class CreateUserDialogBase : ComponentBase
    {
        protected bool IsLoading { get; set; }
        protected CreateUserFormModel FormModel { get; } = new();

        [CascadingParameter]
        protected MudDialogInstance MudDialog { get; set; } = null!;

        protected async Task Submit(EditContext context)
        {
            IsLoading = true;
            //StateHasChanged();
            await Task.Delay(5000);
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
