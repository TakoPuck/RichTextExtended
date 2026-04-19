using RichTextExtended.Source.Parser;
using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class WaveEffect : TextEffect
{
    public const string TAG = "wv";

    public override string TagName => TAG;

    public WaveMode Mode { get; set; }

    public float Frequency { get; set; }

    public float Amplitude { get; set; }

    public float Phase { get; set; }


    public static WaveEffect Create(OpenTagToken token)
    {
        return new()
        {
            Mode = ParserHelper.ParseWaveMode(token.GetArg(0), WaveMode.Vertical),
            Frequency = ParserHelper.ParseFloat(token.GetArg(1), 8f),
            Amplitude = ParserHelper.ParseFloat(token.GetArg(2), 2f),
            Phase = ParserHelper.ParseFloat(token.GetArg(3), 1f)
        };
    }
}
