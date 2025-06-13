// See https://aka.ms/new-console-template for more information
using Ziewaar.Common.Aardvargs;
using Ziewaar.Util.PdfBabe;

internal class Program
{
    private static void Main(string[] args)
    {
        var parsedArgs = ArgParser.Parse(args);
        var settings = new SettingsObject(parsedArgs);
        var convertor =
            new PdfToJsonConvertor(
                new PdfLinesFactory(settings.VerticalLineResolution),
                new RowCellStringFactory(settings.GapTollerance),
                new SemiStructuredDictionaryFactory(
                    settings.EnableDelimiterTuples,
                    settings.TupleDelimiter,
                    settings.TryTitledTuples,
                    settings.Replacements));

        var indentSettings = settings.JsonIndent
            ? Newtonsoft.Json.Formatting.Indented
            : Newtonsoft.Json.Formatting.None;

        Console.WriteLine($"converting {parsedArgs.Filenames} files...");
        foreach (var item in parsedArgs.Filenames)
        {
            try
            {
                string jsonText = "";
                Console.WriteLine($"reading {item}");
                using (var or = File.OpenRead(item))
                {
                    jsonText = convertor.Convert(or, indentSettings);
                }
                Console.WriteLine($"converted {item}");
                using (var wr = new StreamWriter($"{item}.json"))
                {
                    wr.Write(jsonText);
                }
                Console.WriteLine($"written {item}.json");
            }
            catch (Exception ex)
            {
#if DEBUG
                throw;
#endif
                Console.WriteLine($"failed {item} due to {ex}");
            }
        }
        Console.WriteLine("done, terminating.");
    }
}