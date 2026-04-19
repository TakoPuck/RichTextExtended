using MonoGame.Extended.BitmapFonts;

namespace RichTextExtended.Source.Assets;

public class FontGroup
{
    public BitmapFont Normal { get; }

    public BitmapFont Bold { get; }

    public BitmapFont Italic { get; }

    public BitmapFont BoldAndItalic { get; }


    public FontGroup(BitmapFont normal, BitmapFont bold = null, BitmapFont italic = null, BitmapFont boldAndItalic = null)
    {
        Normal = normal;
        Bold = bold ?? normal;
        Italic = italic ?? normal;
        BoldAndItalic = boldAndItalic ?? normal;
    }

    public BitmapFont GetFont(bool bold, bool italic)
    {
        if (bold && italic)
        {
            return BoldAndItalic;
        }

        if (bold)
        {
            return Bold;
        }

        if (italic)
        {
            return Italic;
        }

        return Normal;
    }
}
