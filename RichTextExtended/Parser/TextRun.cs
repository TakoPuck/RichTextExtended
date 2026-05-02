using RichTextExtended.TextEffects;
using System;
using System.Collections.Generic;

namespace RichTextExtended.Parser;

public class TextRun
{
    public string Text { get; }

    public Dictionary<Type, TextEffect> Effects { get; }


    public TextRun(string text, Dictionary<Type, TextEffect> effects)
    {
        Text = text;
        Effects = effects;
    }

    public bool TryGetEffect<T>(out T effect) where T : TextEffect
    {
        if (Effects.TryGetValue(typeof(T), out var e))
        {
            effect = (T)e;
            return true;
        }

        effect = null;
        return false;
    }

    public bool HasEffect<T>() where T : TextEffect
    {
        return Effects.ContainsKey(typeof(T));
    }
}
