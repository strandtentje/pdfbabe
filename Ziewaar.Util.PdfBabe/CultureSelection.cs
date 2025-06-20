using System;
using System.Globalization;

namespace Ziewaar.Util.PdfBabe;

public static class CultureSelection
{
    public static IFormatProvider CurrentDecimalFormat { get; set; } = CultureInfo.CurrentUICulture;
    public static void SetToInvariant() => CurrentDecimalFormat = CultureInfo.InvariantCulture;
    public static void SetToAustrian() => CurrentDecimalFormat = CultureInfo.GetCultureInfo("de-AT");
    public static void SetToGerman() => CurrentDecimalFormat = CultureInfo.GetCultureInfo("de-DE");
    public static void SetToDutch() => CurrentDecimalFormat = CultureInfo.GetCultureInfo("nl-NL");
    public static void SetToEnglish() => CurrentDecimalFormat = CultureInfo.GetCultureInfo("en-US");
    public static void SetToSpecific(string code) => CurrentDecimalFormat = CultureInfo.GetCultureInfo(code.Trim().Replace('_', '-')); 
}