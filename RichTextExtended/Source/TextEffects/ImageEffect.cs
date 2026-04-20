using MonoGame.Extended.Graphics;
using RichTextExtended.Source.Parser;
using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class ImageEffect : TextEffect
{
    public const string TAG = "img";

    public override string TagName => TAG;

    public Texture2DRegion TexRegion { get; set; }


    public static ImageEffect Create(OpenTagToken token)
    {
        return new()
        {
            TexRegion = ParserHelper.ParseImage(token.GetArg(0), null)
        };
    }
}
