using Microsoft.Xna.Framework;
using RichTextExtended.Source.Parser;
using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class TransitionColorEffect : TextEffect
{
    public const string TAG = "tc";

    public override string TagName => TAG;

    public Color Color { get; set; }

    public TransitionMode Mode { get; set; }

 
    public static TransitionColorEffect Create(OpenTagToken token)
    {
        return new()
        {
            Color = ParserHelper.ParseColor(token.GetArg(0), Color.Transparent),
            Mode = ParserHelper.ParseTransitionMode(token.GetArg(1), TransitionMode.In)
        };
    }
}
