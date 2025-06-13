using System.Collections.Generic;
using System.Linq;
using System.Text;
using UglyToad.PdfPig.Content;

namespace Ziewaar.Util.PdfBabe;

public class TabulatedGroup(IEnumerable<Word> words) : List<WrappedWord>(words.Select(x => new WrappedWord(x)))
{
    public override string ToString() =>
        this.Aggregate(
            new StringBuilder(),
            (acc, ww) => acc.Append(ww.Text).Append(" "),
            acc => acc.ToString().TrimEnd());
}
