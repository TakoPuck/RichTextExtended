using System.Collections.Generic;
using System.Text;

namespace RichTextExtended.Source.Scanner;

public class Scanner
{
    private readonly StringBuilder _sbText = new();
    private readonly StringBuilder _sbTag = new();

    private List<Segment> _segments;
    private bool _tagOpened;
    private bool _isProtected;
    private bool _beforeEqual = true;
    private bool _hasLeftPart;
    private bool _hasRightPart;
    private bool _isCloseTag;

    private static bool IsNextCharTagOpener(string input, int i)
        => i < input.Length - 1 && input[i + 1] == '<';

    private static bool IsTagOpenerProtection(string input, int i)
        => i < input.Length - 2 && input[i] == '\\' && IsNextCharTagOpener(input, i) && !IsNextCharTagOpener(input, i + 1);

    private static bool IsCharValidLeft(char c)
        => char.IsAsciiLetter(c) // color
        || c == '-';             // in-color

    private static bool IsCharValidRight(char c)
        => char.IsAsciiDigit(c)  // 1
        || char.IsAsciiLetter(c) // red
        || c == ' '              // 1 1
        || c == '#'              // #001100
        || c == '.';             // 0.1

    private bool ValidateCharInTag(char c)
    {
        if (c == '/')
        {
            if (_isCloseTag || _hasLeftPart) return false;

            _isCloseTag = true;
            return true;
        }
        
        if (c == '=')
        {
            if (!_beforeEqual || !_hasLeftPart) return false;

            _beforeEqual = false;
            return true;
        }

        if (_beforeEqual)
        {
            bool isValidLeft = IsCharValidLeft(c);
            _hasLeftPart |= isValidLeft;
            return isValidLeft;
        }

        bool isValidRight = IsCharValidRight(c);
        _hasRightPart |= isValidRight;
        return isValidRight;
    }

    private bool IsTagValid()
    {
        return _isCloseTag
            ? (_hasLeftPart && _beforeEqual && !_hasRightPart)
            : (_hasLeftPart && !_beforeEqual && _hasRightPart);
    }

    private void ResetFlags()
    {
        _isProtected = _tagOpened = _hasLeftPart = _hasRightPart = _isCloseTag = false;
        _beforeEqual = true;
    }

    private void Flush(StringBuilder sb, SegmentType type)
    {
        Segment s = new(type, sb.ToString());
        _segments.Add(s);
        sb.Clear();
    }

    private void FlushText()
    {
        if (_sbText.Length <= 0) return;

        Flush(_sbText, SegmentType.Text);
    }

    private void FlushTag()
    {
        var type = _sbTag[1] == '/' ? SegmentType.CloseTag : SegmentType.OpenTag;

        // Remove '<' or '</' from tag
        _sbTag.Remove(0, type == SegmentType.CloseTag ? 2 : 1);

        Flush(_sbTag, type);
    }

    private void EmptyTagIntoText()
    {
        _sbText.Append(_sbTag);
        _sbTag.Clear();
    }

    public List<Segment> Scan(string input)
    {
        _segments = [];

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (_tagOpened)
            {
                if (c != '>' && ValidateCharInTag(c))
                {
                    _sbTag.Append(c);
                    continue;
                }

                bool consumedProtection = false;
                if (c == '>' && IsTagValid())
                { 
                    if (_isProtected)
                    {
                        consumedProtection = true;
                    }
                    else
                    {
                        FlushText();
                        FlushTag();
                        ResetFlags();
                        continue;
                    }
                }

                if (!consumedProtection && _isProtected)
                {
                    _sbText.Append('\\');
                }

                EmptyTagIntoText();
                ResetFlags();
            }

            if (c == '<' && !IsNextCharTagOpener(input, i))
            {
                _tagOpened = true;
                _sbTag.Append(c);
            }
            else if (!IsTagOpenerProtection(input, i))
            {
                _sbText.Append(c);
            }
            else
            {
                _isProtected = true;
            }
        }

        EmptyTagIntoText();
        FlushText();

        return _segments;
    }
}
