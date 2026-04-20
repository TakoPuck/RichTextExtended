using RichTextExtended.Source.Assets;
using RichTextExtended.Source.Parser;
using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

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
