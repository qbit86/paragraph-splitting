using System;
using System.Linq;

namespace ParagraphSplitting;

public class ParagraphSplitterTests
{
    [Fact]
    public void SplitParagraph_EmptyInput_ReturnsEmpty()
    {
        var result = ParagraphSplitter.SplitParagraph([], 1, 10);

        Assert.Empty(result);
    }

    [Fact]
    public void SplitParagraph_SingleWord_ReturnsSingleLine()
    {
        int[] wordWidths = [5];
        var result = ParagraphSplitter.SplitParagraph(wordWidths, 1, 10);

        Assert.Equal([0], result);
    }

    [Fact]
    public void SplitParagraph_TwoWordsFitOnOneLine_ReturnsSingleLine()
    {
        int[] wordWidths = [3, 4]; // 3 + 1 + 4 = 8 <= 10
        var result = ParagraphSplitter.SplitParagraph(wordWidths, 1, 10);

        Assert.Equal([1], result);
    }

    [Fact]
    public void SplitParagraph_TwoWordsNeedTwoLines_ReturnsTwoLines()
    {
        int[] wordWidths = [6, 6]; // 6 + 1 + 6 = 13 > 10
        var result = ParagraphSplitter.SplitParagraph(wordWidths, 1, 10);

        Assert.Equal([0, 1], result);
    }

    [Fact]
    public void SplitParagraph_MultipleLines_ReturnsCorrectIndices()
    {
        int[] wordWidths = [4, 3, 5, 2, 4]; // [4,3] [5,2] [4]
        var result = ParagraphSplitter.SplitParagraph(wordWidths, 1, 8);

        Assert.Equal([1, 3, 4], result);
    }

    [Fact]
    public void SplitParagraph_ExactFit_WorksCorrectly()
    {
        int[] wordWidths = [4, 4]; // 4 + 1 + 4 = 9
        var result = ParagraphSplitter.SplitParagraph(wordWidths, 1, 9);

        Assert.Equal([1], result);
    }

    [Fact]
    public void SplitParagraph_SingleWordPerLine_ReturnsAllIndices()
    {
        int[] wordWidths = [5, 5, 5]; // Each word needs its own line with pageWidth = 5
        var result = ParagraphSplitter.SplitParagraph(wordWidths, 1, 5);

        Assert.Equal([0, 1, 2], result);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void SplitParagraph_InvalidPageWidth_ThrowsArgumentOutOfRangeException(int pageWidth)
    {
        int[] wordWidths = [1, 2, 3];

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ParagraphSplitter.SplitParagraph(wordWidths, 1, pageWidth));
    }

    [Fact]
    public void SplitParagraph_NegativeSpaceWidth_ThrowsArgumentOutOfRangeException()
    {
        int[] wordWidths = [1, 2, 3];

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ParagraphSplitter.SplitParagraph(wordWidths, -1, 10));
    }

    [Fact]
    public void SplitParagraph_NullWordWidths_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(() =>
            ParagraphSplitter.SplitParagraph(null!, 1, 10));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void SplitParagraph_InvalidWordWidth_ThrowsArgumentOutOfRangeException(int invalidWordWidth)
    {
        int[] wordWidths = [1, invalidWordWidth, 3];

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            ParagraphSplitter.SplitParagraph(wordWidths, 1, 10));
    }

    [Fact]
    public void SplitParagraph_SingleWordExceedsPageWidth_HandlesGracefully()
    {
        int[] wordWidths = [15]; // Word width (15) > page width (10)
        var result = ParagraphSplitter.SplitParagraph(wordWidths, 1, 10);

        // Should place oversized word on its own line
        Assert.Equal([0], result);
    }

    [Fact]
    public void SplitParagraph_MultipleOversizedWords_HandlesGracefully()
    {
        int[] wordWidths = [12, 15, 8]; // The first two words exceed page width (10)
        var result = ParagraphSplitter.SplitParagraph(wordWidths, 1, 10);

        // Each oversized word should be on its own line, the last word fits normally
        Assert.Equal([0, 1, 2], result);
    }

    [Fact]
    public void SplitParagraph_ZeroSpaceWidth_WorksCorrectly()
    {
        int[] wordWidths = [4, 4, 2]; // With 0 space width: [4,4] [2]
        var result = ParagraphSplitter.SplitParagraph(wordWidths, 0, 8);

        Assert.Equal([1, 2], result);
    }

    [Fact]
    public void SplitParagraph_WordExactlyEqualsPageWidth_WorksCorrectly()
    {
        int[] wordWidths = [10]; // Word width exactly equals page width
        var result = ParagraphSplitter.SplitParagraph(wordWidths, 1, 10);

        Assert.Equal([0], result);
    }

    [Fact]
    public void SplitParagraph_LargeNumberOfWords_PerformanceTest()
    {
        int[] wordWidths = [.. Enumerable.Repeat(3, 1000)]; // 1000 words of width 3
        var result = ParagraphSplitter.SplitParagraph(wordWidths, 1, 10);

        // Each line fits 2 words (3+1+3=7 <= 10), so 500 lines expected
        Assert.Equal(500, result.Count());
    }
}
