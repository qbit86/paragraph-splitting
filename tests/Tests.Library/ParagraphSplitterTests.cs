using System;

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
}
