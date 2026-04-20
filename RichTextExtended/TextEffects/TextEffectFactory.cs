using RichTextExtended.Tokenizer;
using System;
using System.Collections.Generic;

namespace RichTextExtended.TextEffects;

public static class TextEffectFactory
{
    private readonly static Dictionary<string, Func<OpenTagToken, TextEffect>> _factories = new()
    {
        [ImageEffect.TAG] = ImageEffect.Create,
        [AreaEffect.TAG] = AreaEffect.Create,
        [FontEffect.TAG] = FontEffect.Create,

        [ShakeEffect.TAG] = ShakeEffect.Create,
        [WaveEffect.TAG] = WaveEffect.Create,
        [HangEffect.TAG] = HangEffect.Create,

        [PaletteEffect.TAG] = PaletteEffect.Create,
        [ShadowEffect.TAG] = ShadowEffect.Create,
        [ColorEffect.TAG] = ColorEffect.Create,

        [StrikethroughEffect.TAG] = StrikethroughEffect.Create,
        [UnderlineEffect.TAG] = UnderlineEffect.Create,
        [ItalicEffect.TAG] = ItalicEffect.Create,
        [BoldEffect.TAG] = BoldEffect.Create,

        [TransitionIntervalEffect.TAG] = TransitionIntervalEffect.Create,
        [TransitionOffsetEffect.TAG] = TransitionOffsetEffect.Create,
        [TransitionColorEffect.TAG] = TransitionColorEffect.Create,
        [TransitionPauseEffect.TAG] = TransitionPauseEffect.Create,
        [TransitionScopeEffect.TAG] = TransitionScopeEffect.Create,
    };


    public static bool TryCreate(OpenTagToken token, out TextEffect effect)
    {
        if (_factories.TryGetValue(token.Name, out var factory))
        {
            effect = factory(token);
            return true;
        }

        effect = null;
        return false;
    }
}
