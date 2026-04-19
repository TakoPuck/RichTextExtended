using Microsoft.Xna.Framework;

namespace RichTextExtended.Source.TextEffects;

public class ColorEffect : TextEffect
{
    public const string TAG = "c";

    public override string TagName => TAG;

    public Color Color { get; set; }
}
