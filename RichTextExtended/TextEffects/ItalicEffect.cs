using RichTextExtended.Tokenizer;

namespace RichTextExtended.TextEffects;

public class ItalicEffect : TextEffect
{
    public const string TAG = "i";

    public override string TagName => TAG;


    public static ItalicEffect Create(OpenTagToken _)
    {
        return new();
    }
}
