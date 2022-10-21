namespace MudBlazorTemplate.Extensions
{
    public static class StringExtensions
    {
        public static bool Includes(
            this string? source,
            string? value,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            if (string.IsNullOrWhiteSpace(source))
                return false;
            if (string.IsNullOrWhiteSpace(value))
                return false;

            return source.Contains(value, comparison);
        }
    }
}
