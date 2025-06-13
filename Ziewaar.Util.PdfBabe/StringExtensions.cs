using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Ziewaar.Util.PdfBabe;

public static class StringExtensions
{
    public static string SanitizeCamelCase(this string unsanitized, Dictionary<string, string> replacements)
    {
        foreach (var replacement in replacements)
            unsanitized = unsanitized.Replace(replacement.Key, replacement.Value);
        unsanitized = HttpUtility.JavaScriptStringEncode(unsanitized);
        return string.Concat(unsanitized.
            Split([' '], StringSplitOptions.RemoveEmptyEntries).
            Select(x => x.Trim().ToLower()).
            Select(x => $"{x[0].ToString().ToUpperInvariant()}{x.Substring(1)}".Trim()));
    }
}
