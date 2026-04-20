using RichTextExtended.Tokenizer;

namespace RichTextExtended.TextEffects;

public class StrikethroughEffect : TextEffect
{
    public const string TAG = "s";

    public override string TagName => TAG;


    public static StrikethroughEffect Create(OpenTagToken _)
    {
        return new();
    }
}
