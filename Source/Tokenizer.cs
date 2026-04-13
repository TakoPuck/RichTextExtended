using System;
using System.Collections.Generic;

namespace RichTextExtended.Source;

public static class Tokenizer
{
    private static IToken BuildToken(Segment segment)
    {
        switch (segment.Type)
        {
            case SegmentType.Text:
                return new TextToken(segment.Value);

            case SegmentType.OpenTag:
                string[] parts = segment.Value.Split('=');
                return new OpenTagToken(parts[0], parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries));

            case SegmentType.CloseTag:
                return new CloseTagToken(segment.Value);

            default:
                throw new ArgumentOutOfRangeException(nameof(segment), segment.Type, "Invalid segment type");
        }
    }

    public static IToken[] Tokenize(List<Segment> segments)
    {
        IToken[] tokens = new IToken[segments.Count];

        for (int i = 0; i < segments.Count; i++)
        {
            tokens[i] = BuildToken(segments[i]);
        }

        return tokens;
    }
}
