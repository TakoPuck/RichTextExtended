using RichTextExtended.Parser;
using RichTextExtended.Tokenizer;

namespace RichTextExtended.TextEffects;

public class RotationEffect : TextEffect
{
    public const string TAG = "r";

    public override string TagName => TAG;

    public float Degrees { get; set; }


    public static RotationEffect Create(OpenTagToken token)
    {
        return new()
        {
            Degrees = ParserHelper.ParseFloat(token.GetArg(0), 0f),
        };
    }
}
