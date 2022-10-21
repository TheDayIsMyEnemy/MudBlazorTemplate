namespace MudBlazorTemplate
{
    public static class Messages
    {
        public const string SuccessfulCreationFormat = "{0} has been created successfully.";
        public const string SuccessfulUpdateFormat = "{0} has been updated successfully.";
        public const string SuccessfulDeletionFormat = "{0} has been deleted successfully.";
        public const string Error = "An error occurred, please try again later.";
    }

    public static class Roles
    {
        public const string Admin = "Administrator";
        public static string[] List = new string[] { Admin };
    }

    public static class Constants
    {
        public const string LoginPath = "/identity/account/login";
        public const string LogoutPath = "/identity/account/logout";
    }

    public static class MaxLengths
    {
        public const int StringColumn = 128;
    }
}
