namespace RichTextExtended.Source.TextEffects;

public class TransitionIntervalEffect : TextEffect
{
    public const string TAG = "ti";

    public override string TagName => TAG;

    public TransitionMode Mode { get; set; }

    public float Interval { get; set; }
}
