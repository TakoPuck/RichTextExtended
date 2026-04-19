using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class FontEffect : TextEffect
{
    public const string TAG = "f";

    public override string TagName => TAG;

    public string Id { get; set; }


    public static FontEffect Create(OpenTagToken token)
    {
        return new()
        {
            Id = token.GetArg(0)
        };
    }
}
