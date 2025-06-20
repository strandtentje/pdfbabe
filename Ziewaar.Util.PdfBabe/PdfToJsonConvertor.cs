using Newtonsoft.Json;
using System.IO;

namespace Ziewaar.Util.PdfBabe;

public class PdfToJsonConvertor(
    PdfLinesFactory lines,
    RowCellStringFactory cells,
    SemiStructuredDictionaryFactory dicts)
{
    public string Convert(Stream pdfData, Newtonsoft.Json.Formatting fmt = Newtonsoft.Json.Formatting.Indented) =>
        JsonConvert.SerializeObject(
            dicts.CreateDictionaryFromColStrings(
                cells.CreateRowCellStringsFromLines(
                    lines.CreateLinesFromPDFStream(pdfData))),
            fmt);

    public object ConvertToObject(Stream pdfData,
        Newtonsoft.Json.Formatting fmt = Newtonsoft.Json.Formatting.Indented) =>
        dicts.CreateDictionaryFromColStrings(
            cells.CreateRowCellStringsFromLines(
                lines.CreateLinesFromPDFStream(pdfData)));
}