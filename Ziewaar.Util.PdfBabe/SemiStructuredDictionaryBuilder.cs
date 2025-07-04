﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;

namespace Ziewaar.Util.PdfBabe;

public class SemiStructuredDictionaryBuilder(Dictionary<string, string> _replacements = null)
{
    Dictionary<string, object> output = new Dictionary<string, object>();
    CompositionMode workingMode = CompositionMode.None;

    object workingObject;
    private string[] tableTitles;
    private string leftTitle;
    private string rightTitle;
    private string lastTextLine;
    private int textBlockCounter = 1;

    Dictionary<string, string> replacements = _replacements ?? new Dictionary<string, string>();

    public void AddTextLine(string textLine)
    {
        if (workingObject is List<string> textList)
            textList.Add(lastTextLine);
        if (WasModeSwitchRequired<List<string>>(CompositionMode.TextLines, out var newList))
        {
            workingObject = newList = new();
        }

        lastTextLine = textLine;
    }

    public void AddNameValueTuple(string key, string stringValue)
    {
        if (WasModeSwitchRequired<Dictionary<string, object>>(CompositionMode.NameValue, out var dict))
            workingObject = dict ??= new Dictionary<string, object>();

        while (dict.ContainsKey(key))
            key += "_";
        object value =
            decimal.TryParse(stringValue, NumberStyles.Any, CultureSelection.CurrentDecimalFormat, out var decimalValue)
                ? decimalValue
                : stringValue.Trim();
        var candidate = key.SanitizeCamelCase(replacements);
        dict.Add(candidate, value);
    }

    public void AddTitledNameValueTuple(string stringLeft, string stringRight)
    {
        if (WasModeSwitchRequired<List<Dictionary<string, object>>>(CompositionMode.TitleNameValue, out var list))
        {
            workingObject = list ??= new List<Dictionary<string, object>>();
            this.leftTitle = stringLeft.SanitizeCamelCase(replacements);
            this.rightTitle = stringRight.SanitizeCamelCase(replacements);
        }
        else
        {
            object left = decimal.TryParse(stringLeft, NumberStyles.Any, CultureSelection.CurrentDecimalFormat, out var leftValue) ? leftValue : stringLeft.Trim();
            object right = decimal.TryParse(stringRight, NumberStyles.Any, CultureSelection.CurrentDecimalFormat, out var rightValue) ? rightValue : stringRight.Trim();
            list.Add(new Dictionary<string, object>()
            {
                { leftTitle, left },
                { rightTitle, right },
            });
        }
    }

    public void AddFullTableRow(string[] multipleWords)
    {
        if (WasModeSwitchRequired<List<Dictionary<string, object>>>(CompositionMode.FullTable, out var table))
        {
            workingObject = table ??= new();

            multipleWords = multipleWords.Select(x => x.SanitizeCamelCase(replacements)).ToArray();

            List<string> definitiveColHeads = new(multipleWords.Length);

            var duplicates = multipleWords.GroupBy(x => x).Where(x => x.Count() > 1).Select(x => x.Key).ToList();

            for (int i = 0; i < multipleWords.Length; i++)
            {
                var currentWord = multipleWords[i];
                if (duplicates.Contains(currentWord))
                {
                    if (i > 0)
                    {
                        definitiveColHeads.Add(multipleWords[i - 1] + currentWord);
                    }
                    else
                    {
                        definitiveColHeads.Add($"c{i}" + currentWord);
                    }
                }
                else
                {
                    definitiveColHeads.Add(currentWord);
                }
            }

            tableTitles = definitiveColHeads.ToArray();
        }
        else
        {
            var newRow = new Dictionary<string, object>();
            for (int i = 0; i < Math.Max(tableTitles.Length, multipleWords.Length); i++)
            {
                var key = tableTitles.ElementAtOrDefault(i) ?? $"col{i}";
                var valueString = multipleWords.ElementAtOrDefault(i) ?? "";
                object value = decimal.TryParse(valueString, NumberStyles.Any, CultureSelection.CurrentDecimalFormat, out var decimalValue) ? decimalValue : valueString.Trim();
                newRow.Add(key, value);
            }

            table.Add(newRow);
        }
    }

    private bool WasModeSwitchRequired<TRequired>(CompositionMode requestedMode, out TRequired req)
    {
        if (workingMode == requestedMode)
        {
            req = (TRequired)workingObject;
            return false;
        }
        else if (workingMode == CompositionMode.TextLines && workingObject != null)
        {
            if (workingObject is List<string> wol && wol.Count > 0)
            {
                output.Add($"text{textBlockCounter++}", workingObject);
            }
        }
        else if (workingObject != null)
        {
            var candidateTitle = (lastTextLine ?? $"text{textBlockCounter++}").SanitizeCamelCase(replacements);
            while (output.ContainsKey(candidateTitle))
                candidateTitle += "_";
            output.Add(candidateTitle, workingObject);
        }

        workingMode = requestedMode;
        workingObject = null;
        req = default(TRequired);
        return true;
    }

    public Dictionary<string, object> GetResult()
    {
        WasModeSwitchRequired<object>(CompositionMode.Done, out var _);
        return output;
    }
}