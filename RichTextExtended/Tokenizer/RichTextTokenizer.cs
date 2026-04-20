using RichTextExtended.Scanner;
using System;
using System.Collections.Generic;

namespace RichTextExtended.Tokenizer;

public static class RichTextTokenizer
{
    private static IToken BuildToken(Segment segment)
    {
        switch (segment.Type)
        {
            case SegmentType.Text:
                return new TextToken(segment.Value);

            case SegmentType.OpenTag:
                {
                    string[] parts = segment.Value.Split('=');
                    string name = parts.Length > 0 ? parts[0] : string.Empty;
                    string[] args = parts.Length > 1 ? parts[1].Split(' ', StringSplitOptions.RemoveEmptyEntries) : [];
                    return new OpenTagToken(name, args);
                }

            case SegmentType.CloseTag:
                return new CloseTagToken(segment.Value);

            default:
                throw new ArgumentOutOfRangeException(nameof(segment), segment.Type, "Invalid segment type");
        }
    }

    public static IToken[] Tokenize(List<Segment> segments)
    {
        if (segments == null) return [];

        IToken[] tokens = new IToken[segments.Count];

        for (int i = 0; i < segments.Count; i++)
        {
            tokens[i] = BuildToken(segments[i]);
        }

        return tokens;
    }
}
