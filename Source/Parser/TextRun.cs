using RichTextExtended.Source.TextEffects;
using System.Collections.Generic;

namespace RichTextExtended.Source.Parser;

public class TextRun
{
    public string Text { get; }

    public List<TextEffect> Effects { get; }



    public TextRun(string text, List<TextEffect> effects)
    {
        Text = text;
        Effects = effects;
    }
}
