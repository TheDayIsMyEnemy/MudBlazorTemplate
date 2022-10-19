namespace MudBlazorTemplate.Extensions
{
    public static class StringExtensions
    {
        public static bool Includes(
            this string? str,
            string? value,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            else if (string.IsNullOrWhiteSpace(value))
                return false;

            return str.Contains(value, comparison);
        }
    }
}
