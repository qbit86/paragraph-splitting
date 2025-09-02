using System;
using System.Collections.Generic;
using System.Linq;

namespace ParagraphSplitting;

public static class ParagraphSplitter
{
    /// <summary>
    /// Splits a paragraph into lines by determining the trailing word index for each line.
    /// </summary>
    /// <param name="wordWidths">The widths of each word in the paragraph.</param>
    /// <param name="spaceWidth">The width of a space character.</param>
    /// <param name="pageWidth">The maximum width allowed for each line.</param>
    /// <returns>An enumerable of trailing word indices for each line.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="wordWidths" /> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="spaceWidth" /> is negative
    /// or <paramref name="pageWidth" /> is negative or zero.
    /// </exception>
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
                if (newLineWidth > pageWidth && currentLineWidth > 0)
                    break;

                currentLineWidth = newLineWidth;
                ++wordIndex;
            } while (wordIndex < wordCount);
        }
    }
}
