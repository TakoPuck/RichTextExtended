using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class StrikethroughEffect : TextEffect
{
    public const string TAG = "s";

    public override string TagName => TAG;


    public static StrikethroughEffect Create(OpenTagToken _)
    {
        return new();
    }
}
