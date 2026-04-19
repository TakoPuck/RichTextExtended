namespace RichTextExtended.Source.TextEffects;

public class TransitionOffsetEffect : TextEffect
{
    public const string TAG = "to";

    public override string TagName => TAG;

    public TransitionMode TransitionMode { get; set; }

    public float X { get; set; }

    public float Y { get; set; }
}
