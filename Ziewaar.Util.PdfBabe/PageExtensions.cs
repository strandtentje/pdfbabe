using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.Core;

namespace Ziewaar.Util.PdfBabe;

public static class PageExtensions
{
    public static PdfPoint FlipTo(this PdfPoint point, Page page) =>
        new PdfPoint(point.X, page.Height - point.Y);
    public static PdfRectangle FlipTo(this PdfRectangle rect, Page page) =>
        new PdfRectangle(rect.BottomLeft.FlipTo(page), rect.TopRight.FlipTo(page));
    public static IEnumerable<Word> GetWordsWithNaturalCoordinates(this Page page) =>
        page.GetWords().Select(x => new Word(
            x.Letters.Select(y => new Letter(
                y.Value, y.GlyphRectangle.FlipTo(page), y.StartBaseLine.FlipTo(page), y.EndBaseLine.FlipTo(page),
                y.Width, y.FontSize, y.Font, y.RenderingMode, y.StrokeColor, y.FillColor, y.PointSize, y.TextSequence)).ToList()));
    public static double GetAverageLetterDistance(this IEnumerable<Letter> canLetters)
    {
        var letters = canLetters.ToArray();

        var sorted = letters.OrderBy(x => x.GlyphRectangle.Centroid.X).ToArray();
        var paired = sorted.Take(sorted.Length - 1).Zip(sorted.Skip(1), (l, r) => (l, r));
        var distances = paired.Select(x => x.r.GlyphRectangle.Centroid.X - x.l.GlyphRectangle.Centroid.X).ToArray();
        return distances.Sum() / distances.Length;
    }
}
