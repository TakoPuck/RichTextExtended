using Microsoft.Xna.Framework;

namespace RichTextExtended.Source.TextEffects;

public class PaletteEffect : TextEffect
{
    public const string TAG = "p";

    public override string TagName => TAG;

    public Color[] Colors { get; set; }

    public float Interval { get; set; }
}
