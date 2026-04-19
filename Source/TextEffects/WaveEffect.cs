namespace RichTextExtended.Source.TextEffects;

public class WaveEffect : TextEffect
{
    public const string TAG = "wv";

    public override string TagName => TAG;

    public WaveMode Mode { get; set; }

    public float Frequency { get; set; }

    public float Amplitude { get; set; }

    public float Phase { get; set; }
}
