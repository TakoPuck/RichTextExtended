namespace RichTextExtended.Source.TextEffects;

public class TransitionScopeEffect : TextEffect
{
    public const string TAG = "ts";

    public override string TagName => TAG;

    public TransitionMode Mode { get; set; }

    public TransitionScope TransitionScope { get; set; }
}
