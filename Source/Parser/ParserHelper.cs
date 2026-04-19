using Microsoft.Xna.Framework;
using MonoGame.Extended;
using RichTextExtended.Source.Banks;
using RichTextExtended.Source.TextEffects;
using System;
using System.Globalization;

namespace RichTextExtended.Source.Parser;

public static class ParserHelper
{
    public static Color ParseColor(string arg, Color defaultValue)
    {
        if (string.IsNullOrEmpty(arg))
        {
            return defaultValue;
        }

        if (BankRegistry.Instance.ColorBank.TryGetValue(arg, out Color color))
        {
            return color;
        }

        try
        {
            return ColorHelper.FromHex(arg);
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public static float ParseFloat(string arg, float defaultValue)
    {
        if (string.IsNullOrEmpty(arg))
        {
            return defaultValue;
        }

        return float.TryParse(arg, NumberStyles.Float, CultureInfo.InvariantCulture, out float result)
            ? result
            : defaultValue;
    }

    public static WaveMode ParseWaveMode(string arg, WaveMode defaultValue)
    {
        return arg switch
        {
            "h" => WaveMode.Horizontal,
            "v" => WaveMode.Vertical,
            _ => defaultValue
        };
    }

    public static TransitionMode ParseTransitionMode(string arg, TransitionMode defaultValue)
    {
        return arg switch
        {
            "i" => TransitionMode.In,
            "o" => TransitionMode.Out,
            "b" => TransitionMode.Both,
            _ => defaultValue
        };
    }

    public static TransitionScope ParseTransitionScope(string arg, TransitionScope defaultValue)
    {
        return arg switch
        {
            "lt" => TransitionScope.Letter,
            "w" => TransitionScope.Word,
            "lg" => TransitionScope.Line,
            "t" => TransitionScope.Text,
            _ => defaultValue
        };
    }
}
