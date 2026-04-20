using RichTextExtended.Tokenizer;

namespace RichTextExtended.TextEffects;

public class UnderlineEffect : TextEffect
{
    public const string TAG = "u";

    public override string TagName => TAG;


    public static UnderlineEffect Create(OpenTagToken _)
    {
        return new();
    }
}
