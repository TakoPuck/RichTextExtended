using RichTextExtended.Source.Parser;
using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class TransitionOffsetEffect : TextEffect
{
    public const string TAG = "to";

    public override string TagName => TAG;

    public float X { get; set; }

    public float Y { get; set; }

    public TransitionMode Mode { get; set; }


    public static TransitionOffsetEffect Create(OpenTagToken token)
    {
        return new()
        {
            X = ParserHelper.ParseFloat(token.GetArg(0), 0f),
            Y = ParserHelper.ParseFloat(token.GetArg(1), 1f),
            Mode = ParserHelper.ParseTransitionMode(token.GetArg(2), TransitionMode.In)
        };
    }
}
