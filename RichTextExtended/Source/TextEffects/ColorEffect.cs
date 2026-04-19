using Microsoft.Xna.Framework;
using RichTextExtended.Source.Parser;
using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class ColorEffect : TextEffect
{
    public const string TAG = "c";

    public override string TagName => TAG;

    public Color Color { get; set; }


    public static ColorEffect Create(OpenTagToken token)
    {
        return new()
        {
            Color = ParserHelper.ParseColor(token.GetArg(0), Color.White)
        };
    }
}
