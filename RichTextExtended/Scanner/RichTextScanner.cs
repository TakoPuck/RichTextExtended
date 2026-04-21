using System.Collections.Generic;
using System.Text;

namespace RichTextExtended.Scanner;

public static class RichTextScanner
{
    private static bool IsNextCharTagOpener(string input, int i)
        => i >= 0
        && i < input.Length - 1
        && input[i + 1] == '<';

    private static bool IsCharValidLeft(char c)
        => char.IsAsciiLetter(c);

    private static bool IsCharValidRight(char c)
        => char.IsAsciiDigit(c)
        || char.IsAsciiLetter(c)
        || c == '-'
        || c == ' '
        || c == '#'
        || c == '.';

    private static bool ValidateCharInTag(ref ScanState state, char c)
    {
        if (c == '!')
        {
            if (state.IsCloseTag || state.IsProtected || state.HasLeftPart) return false;
            state.IsProtected = true;
            return true;
        }

        if (c == '/')
        {
            if (state.IsProtected || state.IsCloseTag || state.HasLeftPart) return false;
            state.IsCloseTag = true;
            return true;
        }

        if (c == '=')
        {
            if (!state.BeforeEqual || !state.HasLeftPart) return false;
            state.BeforeEqual = false;
            return true;
        }

        if (state.BeforeEqual)
        {
            bool isValidLeft = IsCharValidLeft(c);
            state.HasLeftPart |= isValidLeft;
            return isValidLeft;
        }

        bool isValidRight = IsCharValidRight(c);
        state.HasRightPart |= isValidRight;
        return isValidRight;
    }

    private static bool IsTagValid(ref ScanState state)
        => !state.IsProtected
        && state.HasLeftPart
        && ((state.BeforeEqual && !state.HasRightPart)
        || (!state.BeforeEqual && state.HasRightPart));

    private static void Flush(ref ScanState state, StringBuilder sb, SegmentType type)
    {
        state.Segments.Add(new Segment(type, sb.ToString()));
        sb.Clear();
    }

    private static void FlushText(ref ScanState state)
    {
        if (state.SbText.Length <= 0) return;
        Flush(ref state, state.SbText, SegmentType.Text);
    }

    private static void FlushTag(ref ScanState state)
    {
        var type = state.SbTag[1] == '/' ? SegmentType.CloseTag : SegmentType.OpenTag;
        state.SbTag.Remove(0, type == SegmentType.CloseTag ? 2 : 1);
        Flush(ref state, state.SbTag, type);
    }

    private static void EmptyTagIntoText(ref ScanState state)
    {
        if (state.IsProtected)
        {
            state.SbTag.Remove(1, 1);
        }

        state.SbText.Append(state.SbTag);
        state.SbTag.Clear();
    }

    public static List<Segment> Scan(string input)
    {
        ScanState state = new();

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (state.TagOpened)
            {
                if (c != '>' && ValidateCharInTag(ref state, c))
                {
                    state.SbTag.Append(c);
                    continue;
                }

                if (c == '>' && IsTagValid(ref state))
                {
                    FlushText(ref state);
                    FlushTag(ref state);
                    state.ResetFlags();
                    continue;
                }

                EmptyTagIntoText(ref state);
                state.ResetFlags();
            }

            if (c == '<' && !IsNextCharTagOpener(input, i))
            {
                state.TagOpened = true;
                state.SbTag.Append(c);
            }
            else
            {
                state.SbText.Append(c);
            }
        }

        EmptyTagIntoText(ref state);
        FlushText(ref state);

        return state.Segments;
    }

    private ref struct ScanState
    {
        public StringBuilder SbText = new();
        public StringBuilder SbTag = new();
        public List<Segment> Segments = [];
        public bool TagOpened;
        public bool IsProtected;
        public bool BeforeEqual = true;
        public bool HasLeftPart;
        public bool HasRightPart;
        public bool IsCloseTag;

        public ScanState()
        { }

        public void ResetFlags()
        {
            IsProtected = TagOpened = HasLeftPart = HasRightPart = IsCloseTag = false;
            BeforeEqual = true;
        }
    }
}
