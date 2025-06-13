using System.Collections.Generic;
using System.Linq;

namespace Ziewaar.Util.PdfBabe;

public class SemiStructuredDictionaryFactory(
        bool enableDelimiterTuples = true,
        char tupleDelimiter = ':',
        bool tryTitledTuples = false,
        Dictionary<string, string> replacements = null)
{
    public Dictionary<string, object> CreateDictionaryFromColStrings(List<string[]> linesWithColStrings)
    {
        var builder = new SemiStructuredDictionaryBuilder(replacements);

        foreach (var multipleWords in linesWithColStrings)
        {
            if (multipleWords.All(x => x.Count(x => x == tupleDelimiter) == 1) && enableDelimiterTuples)
            {
                foreach (var currentWord in multipleWords)
                {
                    var members = currentWord.Split(tupleDelimiter);
                    builder.AddNameValueTuple(members.ElementAtOrDefault(0), members.ElementAtOrDefault(1));
                }
            }
            else if (multipleWords.Length == 1)
            {
                builder.AddTextLine(multipleWords.Single());
            }
            else if (multipleWords.Length == 2)
            {
                if (tryTitledTuples)
                {
                    builder.AddTitledNameValueTuple(multipleWords.ElementAt(0), multipleWords.ElementAt(1));
                } else
                {
                    builder.AddNameValueTuple(multipleWords.ElementAt(0), multipleWords.ElementAt(2));
                }
            }
            else if (multipleWords.Length > 2)
            {
                builder.AddFullTableRow(multipleWords);
            }
        }

        return builder.GetResult();
    }



}
