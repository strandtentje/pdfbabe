using System.Collections.Generic;
using System.Linq;
using UglyToad.PdfPig.Content;

namespace Ziewaar.Util.PdfBabe;

public class PdfLine(double top, IEnumerable<Word> words) : List<Word>(words)
{
    public TabulatedGroup[] GetTabulated(double gapTollerance)
    {
        if (this.Count < 2) return [new(this)];

        var wordCouples = this.
            Take(this.Count - 1).
            Zip(this.Skip(1), (l,r) => WordCouple.Create(l,r,gapTollerance)).ToArray();

        List<TabulatedGroup> result = new();
        TabulatedGroup head = new([wordCouples.First().Left]);

        foreach (var item in wordCouples)
        {
            if (item.IsGapBig)
            {
                result.Add(head);
                head = new TabulatedGroup([item.Right]);
            }
            else
            {
                head.Add(new WrappedWord(item.Right));
            }
        }
        if (head.Any()) result.Add(head);
        return result.ToArray();
    }
}
