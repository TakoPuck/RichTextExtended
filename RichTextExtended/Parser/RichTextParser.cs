using RichTextExtended.TextEffects;
using RichTextExtended.Tokenizer;
using System;
using System.Collections.Generic;

namespace RichTextExtended.Parser;

public static class RichTextParser
{
    private static Dictionary<Type, TextEffect> BuildEffectMap(List<TextEffect> effects)
    {
        Dictionary<Type, TextEffect> map = [];

        foreach (var effect in effects)
        {
            map[effect.GetType()] = effect;
        }

        return map;
    }

    public static List<TextRun> Parse(IToken[] tokens)
    {
        List<TextEffect> activeEffects = [];
        List<TextRun> runs = [];

        for (int i = 0; i < tokens.Length; i++)
        {
            IToken token = tokens[i];

            switch (token)
            {
                case OpenTagToken openTag:
                    {
                        if (!TextEffectFactory.TryCreate(openTag, out TextEffect effect)) break;

                        if (effect is ImageEffect || effect is TransitionPauseEffect)
                        {
                            var map = BuildEffectMap(activeEffects);
                            map[effect.GetType()] = effect;
                            TextRun run = new(string.Empty, map);
                            runs.Add(run);
                        }
                        else
                        {
                            activeEffects.Add(effect);
                        }
                        break;
                    }

                case CloseTagToken closeTag:
                    {
                        TextEffect toRemoved = activeEffects.FindLast(e => e.TagName == closeTag.Name);
                        if (toRemoved != null)
                        {
                            activeEffects.Remove(toRemoved);
                        }
                        break;
                    }

                case TextToken text:
                    {
                        TextRun run = new(text.Text, BuildEffectMap(activeEffects));
                        runs.Add(run);
                        break;
                    }

                default:
                    throw new InvalidOperationException($"Unknown token type: {token.GetType().Name}");
            }
        }

        activeEffects.Clear();

        return runs;
    }
}
