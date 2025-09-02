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
        ArgumentOutOfRangeException.ThrowIfNegative(pageWidth);

        if (wordWidths.Count is 0)
            return [];

        List<int> leadingWordIndices = [];
        SplitParagraph(wordWidths, spaceWidth, pageWidth, leadingWordIndices);
        var trailingWordIndices = leadingWordIndices.Select(it => it - 1).Skip(1).Append(wordWidths.Count - 1);
        return trailingWordIndices;
    }

    private static void SplitParagraph(
        IReadOnlyList<int> wordWidths, int spaceWidth, int pageWidth, List<int> leadingWordIndices) =>
        throw new NotImplementedException();
}
