using Microsoft.Xna.Framework;
using RichTextExtended.Banks;
using RichTextExtended.Parser;
using RichTextExtended.Tokenizer;
using System.Globalization;

namespace RichTextExtended.TextEffects;

public class PaletteEffect : TextEffect
{
    public const string TAG = "p";

    public override string TagName => TAG;

    public Color[] Colors { get; set; }

    public float Interval { get; set; }


    public static PaletteEffect Create(OpenTagToken token)
    {
        var args = token.Args;
        
        float interval = 0f;
        bool hasInterval = args.Length > 0 && float.TryParse(args[^1], NumberStyles.Float, CultureInfo.InvariantCulture, out interval);
        int colorCount = args.Length - (hasInterval ? 1 : 0);

        if (!BankRegistry.Instance.PaletteBank.TryGetValue(token.GetArg(0), out Color[] colors))
        {
            if (colorCount <= 0)
            {
                colors = [Color.White];
            }
            else
            {
                colors = new Color[colorCount];
                for (int i = 0; i < colorCount; i++)
                {
                    colors[i] = ParserHelper.ParseColor(args[i], Color.White);
                }
            }
        }

        return new()
        {
            Colors = colors,
            Interval = hasInterval ? interval : 0.075f
        };
    }
}
