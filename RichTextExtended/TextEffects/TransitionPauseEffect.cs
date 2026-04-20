using RichTextExtended.Parser;
using RichTextExtended.Tokenizer;

namespace RichTextExtended.TextEffects;

public class TransitionPauseEffect : TextEffect
{
    public const string TAG = "tp";

    public override string TagName => TAG;

    public float Duration { get; set; }

    public TransitionMode Mode { get; set; }


    public static TransitionPauseEffect Create(OpenTagToken token)
    {
        return new()
        {
            Duration = ParserHelper.ParseFloat(token.GetArg(0), 1f),
            Mode = ParserHelper.ParseTransitionMode(token.GetArg(1), TransitionMode.In)
        };
    }
}
