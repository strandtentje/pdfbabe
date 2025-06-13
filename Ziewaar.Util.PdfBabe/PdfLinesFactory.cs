using System;
using System.IO;
using System.Linq;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;

namespace Ziewaar.Util.PdfBabe;

public class PdfLinesFactory(double verticalLineResolution = 5)
{
    public PdfLine[] CreateLinesFromPDFStream(Stream data)
    {
        using (var doc = PdfDocument.Open(data))
        {
            return doc.
                GetPage(1).
                GetWordsWithNaturalCoordinates().
                GroupBy(
                x => Math.Round(x.BoundingBox.Top / verticalLineResolution),
                (top, words) => new PdfLine(top, words.OfType<Word>().OrderBy(x => x.BoundingBox.Left))).
                OrderBy(x => x.FirstOrDefault().BoundingBox.Top).ToArray();
        }
    }
}
