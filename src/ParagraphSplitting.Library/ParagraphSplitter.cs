using System;
using System.Collections.Generic;
using System.Linq;

namespace ParagraphSplitting;

public static class ParagraphSplitter
{
    public static IEnumerable<int> SplitParagraph(IReadOnlyList<int> wordWidths, int spaceWidth, int pageWidth)
    {
        ArgumentNullException.ThrowIfNull(wordWidths);
        ArgumentOutOfRangeException.ThrowIfNegative(spaceWidth);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pageWidth);

        if (wordWidths.Count is 0)
            return [];

        List<int> leadingWordIndices = [];
        SplitParagraph(wordWidths, spaceWidth, pageWidth, leadingWordIndices);
        var trailingWordIndices = leadingWordIndices.Select(it => it - 1).Skip(1).Append(wordWidths.Count - 1);
        return trailingWordIndices;
    }

    private static void SplitParagraph(
        IReadOnlyList<int> wordWidths, int spaceWidth, int pageWidth, List<int> leadingWordIndices)
    {
        int wordCount = wordWidths.Count;
        for (int wordIndex = 0; wordIndex < wordCount;)
        {
            int leadingWordIndex = wordIndex;
            leadingWordIndices.Add(leadingWordIndex);

            int currentLineWidth = 0;
            do
            {
                int wordWidth = wordWidths[wordIndex];
                ArgumentOutOfRangeException.ThrowIfNegativeOrZero(wordWidth);
                int widthToAppend = currentLineWidth is 0 ? wordWidth : spaceWidth + wordWidth;
                int newLineWidth = currentLineWidth + widthToAppend;
                if (newLineWidth > pageWidth)
                    break;

                currentLineWidth = newLineWidth;
                ++wordIndex;
            } while (wordIndex < wordCount);
        }
    }
}
