using Microsoft.Xna.Framework;
using RichTextExtended.Parser;
using RichTextExtended.Tokenizer;

namespace RichTextExtended.TextEffects;

public class ShadowEffect : TextEffect
{
    public const string TAG = "sd";

    public override string TagName => TAG;

    public Color Color { get; set; }

    public float X { get; set;  }

    public float Y { get; set; }


    public static ShadowEffect Create(OpenTagToken token)
    {
        return new()
        {
            Color = ParserHelper.ParseColor(token.GetArg(0), Color.Gray),
            X = ParserHelper.ParseFloat(token.GetArg(1), -1f),
            Y = ParserHelper.ParseFloat(token.GetArg(2), 1f)
        };
    }
}
