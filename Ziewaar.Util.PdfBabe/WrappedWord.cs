using System;
using UglyToad.PdfPig.Content;

namespace Ziewaar.Util.PdfBabe;

public class WrappedWord(Word word)
{
    public int X => (int)Math.Round(word.BoundingBox.Left);
    public int Y => (int)Math.Round(word.BoundingBox.Top);
    public string Text => word.Text;
}
