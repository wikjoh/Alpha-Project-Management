using System.Text.RegularExpressions;

namespace Presentation.WebApp.Utilities;

public static class HtmlUtils
{
    public static string StripHtml(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        return Regex.Replace(input, "<.*?>", string.Empty);
    }
}