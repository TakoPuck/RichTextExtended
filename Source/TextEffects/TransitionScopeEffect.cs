using RichTextExtended.Source.Parser;
using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class TransitionScopeEffect : TextEffect
{
    public const string TAG = "ts";

    public override string TagName => TAG;

    public TransitionScope Scope { get; set; }

    public TransitionMode Mode { get; set; }


    public static TransitionScopeEffect Create(OpenTagToken token)
    {
        return new()
        {
            Scope = ParserHelper.ParseTransitionScope(token.GetArg(0), TransitionScope.Letter),
            Mode = ParserHelper.ParseTransitionMode(token.GetArg(1), TransitionMode.In)
        };
    }
}
