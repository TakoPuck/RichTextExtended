using RichTextExtended.Source.Parser;
using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class WaveEffect : TextEffect
{
    public const string TAG = "w";

    public override string TagName => TAG;

    public float Frequency { get; set; }

    public float Amplitude { get; set; }

    public float Phase { get; set; }

    public WaveMode Mode { get; set; }


    public static WaveEffect Create(OpenTagToken token)
    {
        return new()
        {
            Frequency = ParserHelper.ParseFloat(token.GetArg(0), 8f),
            Amplitude = ParserHelper.ParseFloat(token.GetArg(1), 2f),
            Phase = ParserHelper.ParseFloat(token.GetArg(2), 1f),
            Mode = ParserHelper.ParseWaveMode(token.GetArg(3), WaveMode.Vertical)
        };
    }
}
