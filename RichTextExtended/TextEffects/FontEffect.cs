using RichTextExtended.Assets;
using RichTextExtended.Parser;
using RichTextExtended.Tokenizer;

namespace RichTextExtended.TextEffects;

public class FontEffect : TextEffect
{
    public const string TAG = "f";

    public override string TagName => TAG;

    public FontGroup FontGroup { get; set; }


    public static FontEffect Create(OpenTagToken token)
    {
        return new()
        {
            FontGroup = ParserHelper.ParseFont(token.GetArg(0), null)
        };
    }
}
