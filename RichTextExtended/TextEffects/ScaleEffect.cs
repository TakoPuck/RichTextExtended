using RichTextExtended.Parser;
using RichTextExtended.Tokenizer;

namespace RichTextExtended.TextEffects;

public class ScaleEffect : TextEffect
{
    public const string TAG = "sc";

    public override string TagName => TAG;

    public float X { get; set; }

    public float Y { get; set; }


    public static ScaleEffect Create(OpenTagToken token)
    {
        return new()
        {
            X = ParserHelper.ParseFloat(token.GetArg(0), 1f),
            Y = ParserHelper.ParseFloat(token.GetArg(1), 1f)
        };
    }
}
