using RichTextExtended.Parser;
using RichTextExtended.Tokenizer;

namespace RichTextExtended.TextEffects;

public class TransitionIntervalEffect : TextEffect
{
    public const string TAG = "ti";

    public override string TagName => TAG;

    public float Interval { get; set; }

    public TransitionMode Mode { get; set; }


    public static TransitionIntervalEffect Create(OpenTagToken token)
    {
        return new()
        {
            Interval = ParserHelper.ParseFloat(token.GetArg(0), 0.05f),
            Mode = ParserHelper.ParseTransitionMode(token.GetArg(1), TransitionMode.In)
        };
    }
}
