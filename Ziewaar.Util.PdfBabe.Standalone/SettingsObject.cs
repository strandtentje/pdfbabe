// See https://aka.ms/new-console-template for more information
using UglyToad.PdfPig.Fonts.AdobeFontMetrics;
using Ziewaar.Common.Aardvargs;

public class SettingsObject(ParsedArgs parsed)
{
    public double VerticalLineResolution => parsed.Options.GetDoubleFrom("vlr", 5);
    public double GapTollerance => parsed.Options.GetDoubleFrom("gt", 1.5);
    public bool EnableDelimiterTuples => parsed.Options.GetBoolFrom("dt", true);
    public char TupleDelimiter => parsed.Options.GetCharFrom("td", ':');
    public bool TryTitledTuples => parsed.Options.GetBoolFrom("tt", true);
    public bool UsingStdIo => parsed.Options.GetBoolFrom("io", false);
    public bool JsonIndent => parsed.Options.GetBoolFrom("pp", true);
    public Dictionary<string, string> Replacements
    {
        get
        {
            var ndict = parsed.Options.Where(x => x.Value is string).ToDictionary(x => x.Key, x => (string)x.Value);
            string[] settingsKeys = ["vlr", "gt", "dt", "td", "tt", "io", "pp"];
            foreach (var item in settingsKeys)
            {
                ndict.Remove(item);
            }
            if (ndict.Count > 0)
            {
                return ndict;
            }
            else
            {
                return new Dictionary<string, string>() {
                    { "%", "pct" }
                };
            }
        }
    }
}