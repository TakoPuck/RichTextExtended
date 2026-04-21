using RichTextExtended.Scanner;

namespace RichTextExtended.Tests;

public class ScannerTests
{
    private static List<Segment> Scan(string input) => RichTextScanner.Scan(input);

    private static void AssertText(Segment s, string value)
    {
        Assert.Equal(SegmentType.Text, s.Type);
        Assert.Equal(value, s.Value);
    }

    private static void AssertOpenTag(Segment s, string value)
    {
        Assert.Equal(SegmentType.OpenTag, s.Type);
        Assert.Equal(value, s.Value);
    }

    private static void AssertCloseTag(Segment s, string value)
    {
        Assert.Equal(SegmentType.CloseTag, s.Type);
        Assert.Equal(value, s.Value);
    }

    [Fact]
    public void EmptyInput()
        => Assert.Empty(Scan(string.Empty));

    [Fact]
    public void HelloWorld()
    {
        var segments = Scan("hello world");
        Assert.Single(segments);
        AssertText(segments[0], "hello world");
    }

    [Fact]
    public void ValidTagWithArgs()
    {
        var segments = Scan("<valid=args>");
        Assert.Single(segments);
        AssertOpenTag(segments[0], "valid=args");
    }

    [Fact]
    public void ValidTagWithHexArg()
    {
        var segments = Scan("<valid=#000000>");
        Assert.Single(segments);
        AssertOpenTag(segments[0], "valid=#000000");
    }

    [Fact]
    public void ValidTagWithDigitArg()
    {
        var segments = Scan("<valid=1>");
        Assert.Single(segments);
        AssertOpenTag(segments[0], "valid=1");
    }

    [Fact]
    public void ValidTagWithDecimalArg()
    {
        var segments = Scan("<valid=1.1>");
        Assert.Single(segments);
        AssertOpenTag(segments[0], "valid=1.1");
    }

    [Fact]
    public void ValidTagWithDashArg()
    {
        var segments = Scan("<valid=valid-args>");
        Assert.Single(segments);
        AssertOpenTag(segments[0], "valid=valid-args");
    }

    [Fact]
    public void ValidOpenAndCloseTagAroundText()
    {
        var segments = Scan("<valid>text</valid>");
        Assert.Equal(3, segments.Count);
        AssertOpenTag(segments[0], "valid");
        AssertText(segments[1], "text");
        AssertCloseTag(segments[2], "valid");
    }

    [Fact]
    public void InvalidTagEmptyArg()
    {
        var segments = Scan("<invalid=>text");
        Assert.Single(segments);
        AssertText(segments[0], "<invalid=>text");
    }

    [Fact]
    public void InvalidTagDoubleSlash()
    {
        var segments = Scan("<//invalid>");
        Assert.Single(segments);
        AssertText(segments[0], "<//invalid>");
    }

    [Fact]
    public void InvalidTagDashInName()
    {
        var segments = Scan("<in-valid>");
        Assert.Single(segments);
        AssertText(segments[0], "<in-valid>");
    }

    [Fact]
    public void InvalidTagLeadingSpace()
    {
        var segments = Scan("< invalid>");
        Assert.Single(segments);
        AssertText(segments[0], "< invalid>");
    }

    [Fact]
    public void InvalidTagCommaInArg()
    {
        var segments = Scan("<invalid=1,2>");
        Assert.Single(segments);
        AssertText(segments[0], "<invalid=1,2>");
    }

    [Fact]
    public void ProtectedTag()
    {
        var segments = Scan("<!protected>");
        Assert.Single(segments);
        AssertText(segments[0], "<protected>");
    }

    [Fact]
    public void ProtectedTagWithDoubleProtection()
    {
        var segments = Scan("<!!protected>");
        Assert.Single(segments);
        AssertText(segments[0], "<!protected>");
    }

    [Fact]
    public void ProtectedCloseTag()
    {
        var segments = Scan("<!/protected>");
        Assert.Single(segments);
        AssertText(segments[0], "</protected>");
    }

    [Fact]
    public void TripleOpener()
    {
        var segments = Scan("<<<valid>");
        Assert.Equal(2, segments.Count);
        AssertText(segments[0], "<<");
        AssertOpenTag(segments[1], "valid");
    }

    [Fact]
    public void UnclosedTag()
    {
        var segments = Scan("<unclosed");
        Assert.Single(segments);
        AssertText(segments[0], "<unclosed");
    }

    [Fact]
    public void EmptyTag()
    {
        var segments = Scan("<>");
        Assert.Single(segments);
        AssertText(segments[0], "<>");
    }

    [Fact]
    public void TagAtStartOfInput()
    {
        var segments = Scan("<tag>text");
        Assert.Equal(2, segments.Count);
        AssertOpenTag(segments[0], "tag");
        AssertText(segments[1], "text");
    }

    [Fact]
    public void TagAtEndOfInput()
    {
        var segments = Scan("text<tag>");
        Assert.Equal(2, segments.Count);
        AssertText(segments[0], "text");
        AssertOpenTag(segments[1], "tag");
    }

    [Fact]
    public void UnclosedTagAtEndOfInput()
    {
        var segments = Scan("text<unclosed");
        Assert.Single(segments);
        AssertText(segments[0], "text<unclosed");
    }
}