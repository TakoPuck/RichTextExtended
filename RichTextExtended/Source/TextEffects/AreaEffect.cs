using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class AreaEffect : TextEffect
{
    public const string TAG = "a";

    public override string TagName => TAG;

    public string Id { get; set; }


    public static AreaEffect Create(OpenTagToken token)
    {
        return new()
        {
            Id = token.GetArg(0)
        };
    }
}
