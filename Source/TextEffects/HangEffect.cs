using RichTextExtended.Source.Parser;
using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class HangEffect : TextEffect
{
    public const string TAG = "hg";

    public override string TagName => TAG;

    public float Frequency { get; set; }

    public float Amplitude { get; set; }

    public float Phase { get; set; }


    public static HangEffect Create(OpenTagToken token)
    {
        return new()
        {
            Frequency = ParserHelper.ParseFloat(token.GetArg(1), 6f),
            Amplitude = ParserHelper.ParseFloat(token.GetArg(2), 9f),
            Phase = ParserHelper.ParseFloat(token.GetArg(3), 1f)
        };
    }
}
