namespace RichTextExtended.Source.TextEffects;

public class TransitionPauseEffect : TextEffect
{
    public const string TAG = "tp";

    public override string TagName => TAG;

    public TransitionMode Mode { get; set; }

    public float Duration { get; set; }
}
