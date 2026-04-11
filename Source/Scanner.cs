using System.Collections.Generic;
using System.Text;

namespace RichTextExtended.Source;

public class Scanner
{
    private readonly StringBuilder _sb = new();

    private List<Segment> _segments;
    private Segment _current;

    // TODO
    //private bool IsTagValid(string tag)
    //{
    //    // <identifier=value>
    //}

    private void Flush()
    {
        if (_current == null) return;

        // Remove '/' from closing tag
        if (_current.Type == SegmentType.TagClose)
        {
            _sb.Remove(0, 1);
        }

        _current.Value = _sb.ToString();
        _sb.Clear();

        _segments.Add(_current);
        _current = null;
    }

    public List<Segment> Scan(string input)
    {
        _segments = [];

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (c == '<' && i < input.Length - 1 && input[i + 1] != '<')
            {
                Flush();
                var type = input[i + 1] == '/' ? SegmentType.TagClose : SegmentType.TagOpen;
                _current = new(type);
            }
            else if (c == '>' && _current != null && _current.Type != SegmentType.Text)
            {
                Flush();
            }
            else
            {
                _current ??= new(SegmentType.Text);
                _sb.Append(c);
            }
        }

        Flush();

        return _segments;
    }
}
