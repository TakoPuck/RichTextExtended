using RichTextExtended.Source.Parser;
using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class ShakeEffect : TextEffect
{
    public const string TAG = "sk";

    public override string TagName => TAG;

    public float Strength { get; set; }

    public float Interval { get; set; }


    public static ShakeEffect Create(OpenTagToken token)
    {
        return new()
        {
            Interval = ParserHelper.ParseFloat(token.GetArg(0), 0.06f),
            Strength = ParserHelper.ParseFloat(token.GetArg(1), 0.75f)
        };
    }
}
