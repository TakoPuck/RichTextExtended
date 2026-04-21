using RichTextExtended.Scanner;
using RichTextExtended.Tokenizer;

namespace RichTextExtended.Tests;

public class TokenizerTests
{
    [Fact]
    public void NullSegments()
    {
        Assert.Empty(RichTextTokenizer.Tokenize(null));
    }

    [Fact]
    public void InvalidSegmentType()
    {
        List<Segment> segments = [new Segment((SegmentType)99, "???")];
        Assert.Throws<ArgumentOutOfRangeException>(() => RichTextTokenizer.Tokenize(segments));
    }

    [Fact]
    public void OpenTagWithMultipleArgs()
    {
        List<Segment> segments = [new Segment(SegmentType.OpenTag, "test=  1.2 1    #001122 in-out   ")];

        OpenTagToken token = Assert.IsType<OpenTagToken>(RichTextTokenizer.Tokenize(segments)[0]);

        Assert.Equal("test", token.Name);
        Assert.Equal(["1.2", "1", "#001122", "in-out"], token.Args);
    }

    [Fact]
    public void FullSequence()
    {
        List<Segment> segments =
        [
            new Segment(SegmentType.OpenTag,  "c=#FF0000"),
            new Segment(SegmentType.Text,     "Hello"),
            new Segment(SegmentType.CloseTag, "c"),
        ];

        IToken[] tokens = RichTextTokenizer.Tokenize(segments);

        Assert.Equal(3, tokens.Length);

        Assert.Equal("c", Assert.IsType<OpenTagToken>(tokens[0]).Name);
        Assert.Single(((OpenTagToken)tokens[0]).Args);
        Assert.Equal("#FF0000", Assert.IsType<OpenTagToken>(tokens[0]).Args[0]);
        Assert.Equal("Hello", Assert.IsType<TextToken>(tokens[1]).Text);
        Assert.Equal("c", Assert.IsType<CloseTagToken>(tokens[2]).Name);
    }
}
