using RichTextExtended.Parser;
using RichTextExtended.Tokenizer;

namespace RichTextExtended.TextEffects;

public class HangEffect : TextEffect
{
    public const string TAG = "h";

    public override string TagName => TAG;

    public float Frequency { get; set; }

    public float Amplitude { get; set; }

    public float Phase { get; set; }


    public static HangEffect Create(OpenTagToken token)
    {
        return new()
        {
            Frequency = ParserHelper.ParseFloat(token.GetArg(0), 6f),
            Amplitude = ParserHelper.ParseFloat(token.GetArg(1), 9f),
            Phase = ParserHelper.ParseFloat(token.GetArg(2), 1f)
        };
    }
}
