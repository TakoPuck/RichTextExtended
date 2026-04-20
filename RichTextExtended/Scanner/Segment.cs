namespace RichTextExtended.Scanner;

public class Segment
{
    public SegmentType Type { get; }
    
    public string Value { get; }


    public Segment(SegmentType type, string value)
    { 
        Type = type;
        Value = value;
    }

    public override string ToString()
    {
        return $"{Type} '{Value}'";
    }
}
