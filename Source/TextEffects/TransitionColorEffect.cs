using Microsoft.Xna.Framework;

namespace RichTextExtended.Source.TextEffects;

public class TransitionColorEffect : TextEffect
{
    public const string TAG = "tc";

    public override string TagName => TAG;

    public TransitionMode Mode { get; set; }

    public Color Color { get; set; }
}
