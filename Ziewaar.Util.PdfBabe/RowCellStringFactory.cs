using System.Collections.Generic;
using System.Linq;

namespace Ziewaar.Util.PdfBabe;

public class RowCellStringFactory(double gapTollerance = 1.5)
{
    public List<string[]> CreateRowCellStringsFromLines(PdfLine[] sentences)
    {
        var linesWithWordsets = sentences.
            Select(x => x.GetTabulated(gapTollerance)).
            OrderBy(x => (x.First().First().Y, x.First().First().X)).
            ToArray();
        var linesWithColStrings = linesWithWordsets.Select(
            rowCellsGroups => rowCellsGroups.Select(cellGroup =>
            cellGroup.ToString()).ToArray());
        return [.. linesWithColStrings];
    }
}
