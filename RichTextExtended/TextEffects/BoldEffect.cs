using RichTextExtended.Tokenizer;

namespace RichTextExtended.TextEffects;

public class BoldEffect : TextEffect
{
    public const string TAG = "b";

    public override string TagName => TAG;


    public static BoldEffect Create(OpenTagToken _)
    {
        return new();
    }
}
