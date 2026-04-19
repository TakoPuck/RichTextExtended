using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class ImageEffect : TextEffect
{
    public const string TAG = "img";

    public override string TagName => TAG;

    public string Id { get; set; }


    public static ImageEffect Create(OpenTagToken token)
    {
        return new()
        {
            Id = token.GetArg(0)
        };
    }
}
