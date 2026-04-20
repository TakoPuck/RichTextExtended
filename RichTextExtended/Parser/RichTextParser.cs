using RichTextExtended.TextEffects;
using RichTextExtended.Tokenizer;
using System;
using System.Collections.Generic;

namespace RichTextExtended.Parser;

public class RichTextParser
{
    private readonly List<TextEffect> _activeEffects = [];


    public List<TextRun> Parse(IToken[] tokens)
    {
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
                            TextRun run = new(string.Empty, [.. _activeEffects, effect]);
                            runs.Add(run);
                        }
                        else
                        {
                            _activeEffects.Add(effect);
                        }
                        break;
                    }

                case CloseTagToken closeTag:
                    {
                        TextEffect toRemoved = _activeEffects.FindLast(e => e.TagName == closeTag.Name);
                        if (toRemoved != null)
                        {
                            _activeEffects.Remove(toRemoved);
                        }
                        break;
                    }

                case TextToken text:
                    {
                        TextRun run = new(text.Text, [.. _activeEffects]);
                        runs.Add(run);
                        break;
                    }

                default:
                    throw new InvalidOperationException($"Unknown token type: {token.GetType().Name}");
            }
        }

        _activeEffects.Clear();

        return runs;
    }
}
