namespace RichTextExtended.Source;

public class Segment
{
    public SegmentType Type { get; }
    
    public string Value { get; set; }


    public Segment(SegmentType type)
    { 
        Type = type;
    }

    public override string ToString()
    {
        return $"{Type} '{Value}'";
    }
}
