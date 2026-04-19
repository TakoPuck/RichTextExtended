using Microsoft.Xna.Framework;
using RichTextExtended.Source.Banks;
using RichTextExtended.Source.Parser;
using RichTextExtended.Source.Tokenizer;

namespace RichTextExtended.Source.TextEffects;

public class PaletteEffect : TextEffect
{
    public const string TAG = "p";

    public override string TagName => TAG;

    public Color[] Colors { get; set; }

    public float Interval { get; set; }


    public static PaletteEffect Create(OpenTagToken token)
    {
        if (!BankRegistry.Instance.PaletteBank.TryGetValue(token.GetArg(0), out Color[] colors))
        {
            int paletteLength = token.Args.Length - 1;
            if (paletteLength <= 0)
            {
                colors = [Color.White];
            }
            else
            {
                colors = new Color[paletteLength];
                for (int i = 0; i < paletteLength; i++)
                {
                    colors[i] = ParserHelper.ParseColor(token.Args[i], Color.White);
                }
            }
        }

        return new()
        {
            Colors = colors,
            Interval = ParserHelper.ParseFloat(token.GetArg(token.Args.Length - 1), 0.075f)
        };
    }
}
