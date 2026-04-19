using Microsoft.Xna.Framework;

namespace RichTextExtended.Source.TextEffects;

public class ShadowEffect : TextEffect
{
    public const string TAG = "sd";

    public override string TagName => TAG;

    public Color Color { get; set; }

    public float X { get; set;  }

    public float Y { get; set; }
}
