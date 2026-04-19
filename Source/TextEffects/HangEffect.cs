namespace RichTextExtended.Source.TextEffects;

public class HangEffect : TextEffect
{
    public const string TAG = "hg";

    public override string TagName => TAG;

    public float Frequency { get; set; }

    public float Amplitude { get; set; }

    public float Phase { get; set; }
}
