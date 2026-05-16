using RichTextExtended.Parser;
using RichTextExtended.Tokenizer;

namespace RichTextExtended.TextEffects;

public class OffsetEffect : TextEffect
{
    public const string TAG = "o";

    public override string TagName => TAG;

    public float X { get; set; }

    public float Y { get; set; }


    public static OffsetEffect Create(OpenTagToken token)
    {
        return new()
        {
            X = ParserHelper.ParseFloat(token.GetArg(0), 0f),
            Y = ParserHelper.ParseFloat(token.GetArg(1), 0f)
        };
    }
}
