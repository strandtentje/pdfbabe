using System;
using UglyToad.PdfPig.Content;

namespace Ziewaar.Util.PdfBabe;

public class WordCouple(Word leftmost, Word rightmost, double gapTollerance) // tollerance advised at 1.5
{
    public Word Left => leftmost;
    public Word Right => rightmost;
    public double AverageLetterDistance
    {
        get
        {
            if (leftmost.Letters.Count > 1 && rightmost.Letters.Count > 1)
            {
                return Math.Abs(leftmost.Letters.GetAverageLetterDistance() / 2) +
                    Math.Abs(rightmost.Letters.GetAverageLetterDistance() / 2) + 1;
            }
            else if (leftmost.Letters.Count > 1)
            {
                return leftmost.Letters.GetAverageLetterDistance();
            }
            else if (rightmost.Letters.Count > 1)
            {
                return rightmost.Letters.GetAverageLetterDistance();
            }
            else
            {
                return 1;
            }
        }
    }

    public double GapSize => rightmost.BoundingBox.Left - leftmost.BoundingBox.Right + 1;
    public double GapRatio => Math.Abs(GapSize) / AverageLetterDistance;
    public bool IsGapBig => GapRatio > gapTollerance && !Left.Text.EndsWith(":");
    public static WordCouple Create(Word first, Word second, double gapTollerance)
    {
        if (first.BoundingBox.Centroid.X > second.BoundingBox.Centroid.X)
        {
            return new WordCouple(second, first, gapTollerance);
        }
        else
        {
            return new WordCouple(first, second, gapTollerance);
        }
    }
}
