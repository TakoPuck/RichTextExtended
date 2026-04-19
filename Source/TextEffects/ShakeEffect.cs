namespace RichTextExtended.Source.TextEffects;

public class ShakeEffect : TextEffect
{
    public const string TAG = "sk";

    public override string TagName => TAG;

    public float Strength { get; set; }

    public float Interval { get; set; }
}
