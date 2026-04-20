using Microsoft.Xna.Framework;
using MonoGame.Extended.Graphics;
using RichTextExtended.Assets;
using RichTextExtended.Banks;
using RichTextExtended.Parser;
using RichTextExtended.Scanner;
using RichTextExtended.TextEffects;
using RichTextExtended.Tokenizer;
using System.Runtime.CompilerServices;

namespace RichTextExtended.Tests;

public class ParserTests
{
    private readonly Color[] _palette;

    public ParserTests()
    {
        var img = (Texture2DRegion)RuntimeHelpers.GetUninitializedObject(typeof(Texture2DRegion));
        BankRegistry.Instance.ImageBank.TryAdd("my-image", img);

        var font = (FontGroup)RuntimeHelpers.GetUninitializedObject(typeof(FontGroup));
        BankRegistry.Instance.FontBank.TryAdd("my-font", font);

        _palette = [Color.Red, Color.Lime, Color.Blue];
        BankRegistry.Instance.PaletteBank.TryAdd("my-palette", _palette);
    }

    private List<TextRun> Parse(string input)
    {
        var segments = RichTextScanner.Scan(input);
        var tokens = RichTextTokenizer.Tokenize(segments);
        return RichTextParser.Parse(tokens);
    }

    #region Parser

    [Fact]
    public void EmptyStringProducesNoRuns()
    {
        var runs = Parse(string.Empty);
        Assert.Empty(runs);
    }

    [Fact]
    public void PlainTextProducesSingleRunWithNoEffects()
    {
        var runs = Parse("hello world");

        Assert.Single(runs);
        Assert.Equal("hello world", runs[0].Text);
        Assert.Empty(runs[0].Effects);
    }

    [Fact]
    public void NestedTagsProduceSingleRunWithMultipleEffects()
    {
        var runs = Parse("<b><i>my text</i></b>");

        Assert.Single(runs);
        Assert.Equal("my text", runs[0].Text);
        Assert.Equal(2, runs[0].Effects.Count);
        Assert.Contains(runs[0].Effects, e => e is BoldEffect);
        Assert.Contains(runs[0].Effects, e => e is ItalicEffect);
    }

    [Fact]
    public void PartiallyOverlappingNestedTagsProduceCorrectRuns()
    {
        var runs = Parse("<b>bold <i>bold-italic</i></b>");

        Assert.Equal(2, runs.Count);

        Assert.Equal("bold ", runs[0].Text);
        Assert.Single(runs[0].Effects);
        Assert.IsType<BoldEffect>(runs[0].Effects[0]);

        Assert.Equal("bold-italic", runs[1].Text);
        Assert.Equal(2, runs[1].Effects.Count);
        Assert.Contains(runs[1].Effects, e => e is BoldEffect);
        Assert.Contains(runs[1].Effects, e => e is ItalicEffect);
    }

    [Fact]
    public void MultipleDisjointTagsPreserveOrder()
    {
        var runs = Parse("<c=red>red</c><c=blue>blue</c><c=green>green</c>");

        Assert.Equal(3, runs.Count);
        Assert.Equal("red", runs[0].Text);
        Assert.Equal("blue", runs[1].Text);
        Assert.Equal("green", runs[2].Text);
        Assert.Equal(Color.Red, runs[0].Effects.OfType<ColorEffect>().Single().Color);
        Assert.Equal(Color.Blue, runs[1].Effects.OfType<ColorEffect>().Single().Color);
        Assert.Equal(Color.Green, runs[2].Effects.OfType<ColorEffect>().Single().Color);
    }

    [Fact]
    public void CrossClosingTagsStillApplyBothEffectsToText()
    {
        var runs = Parse("<c=red><b>hi</c></b>");

        Assert.Single(runs);
        Assert.Equal("hi", runs[0].Text);
        Assert.Equal(2, runs[0].Effects.Count);
        Assert.Contains(runs[0].Effects, e => e is ColorEffect);
        Assert.Contains(runs[0].Effects, e => e is BoldEffect);
    }

    [Fact]
    public void CrossClosingTagsLeaveNoResidualEffectsAfterBothClosed()
    {
        var runs = Parse("<c=red><b>hi</c></b> after");

        Assert.Equal(2, runs.Count);
        Assert.Equal("hi", runs[0].Text);
        Assert.Equal(" after", runs[1].Text);
        Assert.Empty(runs[1].Effects);
    }

    [Fact]
    public void SameTagNestedTwiceRemovesOnlyInnermostOnFirstClose()
    {
        var runs = Parse("<b><b>text</b> outer</b>");

        Assert.Equal(2, runs.Count);

        Assert.Equal("text", runs[0].Text);
        Assert.Equal(2, runs[0].Effects.Count);
        Assert.All(runs[0].Effects, e => Assert.IsType<BoldEffect>(e));

        Assert.Equal(" outer", runs[1].Text);
        Assert.Single(runs[1].Effects);
        Assert.IsType<BoldEffect>(runs[1].Effects[0]);
    }

    [Fact]
    public void UnclosedTagStillAppliesEffectToText()
    {
        var runs = Parse("<b>hello");

        Assert.Single(runs);
        Assert.Equal("hello", runs[0].Text);
        Assert.Single(runs[0].Effects);
        Assert.IsType<BoldEffect>(runs[0].Effects[0]);
    }

    [Fact]
    public void CloseTagWithoutMatchingOpenTagIsIgnored()
    {
        var runs = Parse("</b>hello");

        Assert.Single(runs);
        Assert.Equal("hello", runs[0].Text);
        Assert.Empty(runs[0].Effects);
    }

    [Fact]
    public void CloseTagWithoutMatchingOpenTagDoesNotAffectActiveEffects()
    {
        var runs = Parse("<i></b>text</i>");

        Assert.Single(runs);
        Assert.Equal("text", runs[0].Text);
        Assert.Single(runs[0].Effects);
        Assert.IsType<ItalicEffect>(runs[0].Effects[0]);
    }

    [Fact]
    public void UnknownTagIsIgnoredAndTextHasNoEffects()
    {
        var runs = Parse("<xyz>hello</xyz>");

        Assert.Single(runs);
        Assert.Equal("hello", runs[0].Text);
        Assert.Empty(runs[0].Effects);
    }

    [Fact]
    public void UnknownTagInsideValidTagDoesNotAffectActiveEffects()
    {
        var runs = Parse("<b><xyz>text</xyz></b>");

        Assert.Single(runs);
        Assert.Equal("text", runs[0].Text);
        Assert.Single(runs[0].Effects);
        Assert.IsType<BoldEffect>(runs[0].Effects[0]);
    }

    [Fact]
    public void ImageEffectProducesImmediateRunWithEmptyText()
    {
        var runs = Parse("<img=my-image>");

        Assert.Single(runs);
        Assert.Equal(string.Empty, runs[0].Text);
        Assert.Single(runs[0].Effects);
        Assert.IsType<ImageEffect>(runs[0].Effects[0]);
    }

    [Fact]
    public void ImageEffectDoesNotPolluteSubsequentTextRuns()
    {
        var runs = Parse("<img=my-image>after");

        Assert.Equal(2, runs.Count);
        Assert.Equal(string.Empty, runs[0].Text);
        Assert.IsType<ImageEffect>(runs[0].Effects[0]);

        Assert.Equal("after", runs[1].Text);
        Assert.Empty(runs[1].Effects);
    }

    [Fact]
    public void TransitionPauseEffectProducesImmediateRunWithEmptyText()
    {
        var runs = Parse("<tp=2>");

        Assert.Single(runs);
        Assert.Equal(string.Empty, runs[0].Text);
        Assert.Single(runs[0].Effects);
        Assert.IsType<TransitionPauseEffect>(runs[0].Effects[0]);
    }

    [Fact]
    public void TransitionPauseEffectDoesNotPolluteSubsequentTextRuns()
    {
        var runs = Parse("<tp=2>after");

        Assert.Equal(2, runs.Count);
        Assert.Equal(string.Empty, runs[0].Text);
        Assert.IsType<TransitionPauseEffect>(runs[0].Effects[0]);

        Assert.Equal("after", runs[1].Text);
        Assert.Empty(runs[1].Effects);
    }

    [Fact]
    public void SelfClosingEffectInheritsActiveEffectsAtCallSite()
    {
        var runs = Parse("<b><img=my-image></b>");

        Assert.Single(runs);
        Assert.Equal(string.Empty, runs[0].Text);
        Assert.Equal(2, runs[0].Effects.Count);
        Assert.Contains(runs[0].Effects, e => e is BoldEffect);
        Assert.Contains(runs[0].Effects, e => e is ImageEffect);
    }

    #endregion
    #region TextEffect

    private T SingleEffect<T>(string input, string expectedText = "") where T : TextEffect
    {
        var runs = Parse(input);

        Assert.Single(runs);
        Assert.Equal(expectedText, runs[0].Text);
        Assert.Single(runs[0].Effects);
        Assert.IsType<T>(runs[0].Effects[0]);

        return runs[0].Effects.OfType<T>().Single();
    }

    [Fact]
    public void SingleArea()
    {
        var effect = SingleEffect<AreaEffect>("<a=my-id>my text</a>", "my text");
        Assert.Equal("my-id", effect.Id);
    }

    [Fact]
    public void SingleAreaWithDefaultValues()
    {
        var effect = SingleEffect<AreaEffect>("<a>my text</a>", "my text");
        Assert.Equal(string.Empty, effect.Id);
    }

    [Fact]
    public void SingleBold()
    {
        SingleEffect<BoldEffect>("<b>my text</b>", "my text");
    }

    [Fact]
    public void SingleColorFromBank()
    {
        var effect = SingleEffect<ColorEffect>("<c=red>my text</c>", "my text");
        Assert.Equal(Color.Red, effect.Color);
    }

    [Fact]
    public void SingleColorFromBankButInvalid()
    {
        var effect = SingleEffect<ColorEffect>("<c=beautiful-red>my text</c>", "my text");
        Assert.Equal(Color.White, effect.Color);
    }

    [Fact]
    public void SingleColorFromHex()
    {
        var effect = SingleEffect<ColorEffect>("<c=#FF0000>my text</c>", "my text");
        Assert.Equal(Color.Red, effect.Color);
    }

    [Fact]
    public void SingleColorFromHexWithAlpha()
    {
        var effect = SingleEffect<ColorEffect>("<c=#FF000010>my text</c>", "my text");
        Assert.Equal(new(255, 0, 0, 16), effect.Color);
    }

    [Fact]
    public void SingleColorFromHexButInvalid()
    {
        var effect = SingleEffect<ColorEffect>("<c=#INVALID>my text</c>", "my text");
        Assert.Equal(Color.White, effect.Color);
    }

    [Fact]
    public void SingleColorWithDefaultValues()
    {
        var effect = SingleEffect<ColorEffect>("<c>my text</c>", "my text");
        Assert.Equal(Color.White, effect.Color);
    }

    [Fact]
    public void SingleFontFromBank()
    {
        var effect = SingleEffect<FontEffect>("<f=my-font>my text</f>", "my text");
        Assert.NotNull(effect.FontGroup);
    }

    [Fact]
    public void SingleFontFromBankButInvalid()
    {
        var effect = SingleEffect<FontEffect>("<f=my-invalid-font>my text</f>", "my text");
        Assert.Null(effect.FontGroup);
    }

    [Fact]
    public void SingleHang()
    {
        var effect = SingleEffect<HangEffect>("<h=1.1 2 3>my text</h>", "my text");
        Assert.Equal(1.1f, effect.Frequency, precision: 4);
        Assert.Equal(2f, effect.Amplitude, precision: 4);
        Assert.Equal(3f, effect.Phase, precision: 4);
    }

    [Fact]
    public void SingleHangWithDefaultValues()
    {
        var effect = SingleEffect<HangEffect>("<h>my text</h>", "my text");
        Assert.Equal(6f, effect.Frequency, precision: 4);
        Assert.Equal(9f, effect.Amplitude, precision: 4);
        Assert.Equal(1f, effect.Phase, precision: 4);
    }

    [Fact]
    public void SingleImageFromBank()
    {
        var effect = SingleEffect<ImageEffect>("<img=my-image>");
        Assert.NotNull(effect.TexRegion);
    }

    [Fact]
    public void SingleImageFromBankButInvalid()
    {
        var effect = SingleEffect<ImageEffect>("<img=my-invalid-image>");
        Assert.Null(effect.TexRegion);
    }

    [Fact]
    public void SingleItalic()
    {
        SingleEffect<ItalicEffect>("<i>my text</i>", "my text");
    }

    [Fact]
    public void SinglePaletteFromBank()
    {
        var effect = SingleEffect<PaletteEffect>("<p=my-palette>my text</p>", "my text");
        Assert.Equal(3, effect.Colors.Length);
        Assert.Equal(Color.Red, effect.Colors[0]);
        Assert.Equal(Color.Lime, effect.Colors[1]);
        Assert.Equal(Color.Blue, effect.Colors[2]);
    }

    [Fact]
    public void SinglePaletteFromBankButInvalid()
    {
        var effect = SingleEffect<PaletteEffect>("<p=my-invalid-palette>my text</p>", "my text");
        Assert.Single(effect.Colors);
        Assert.Equal(Color.White, effect.Colors[0]);
    }

    [Fact]
    public void SinglePaletteFromHexColors()
    {
        var effect = SingleEffect<PaletteEffect>("<p=#FF0000 #00FF00 #0000FF>my text</p>", "my text");
        Assert.Equal(3, effect.Colors.Length);
        Assert.Equal(Color.Red, effect.Colors[0]);
        Assert.Equal(Color.Lime, effect.Colors[1]);
        Assert.Equal(Color.Blue, effect.Colors[2]);
    }

    [Fact]
    public void SinglePaletteFromHexColorsWithInterval()
    {
        var effect = SingleEffect<PaletteEffect>("<p=#FF0000 #00FF00 0.2>my text</p>", "my text");
        Assert.Equal(2, effect.Colors.Length);
        Assert.Equal(Color.Red, effect.Colors[0]);
        Assert.Equal(Color.Lime, effect.Colors[1]);
        Assert.Equal(0.2f, effect.Interval, precision: 4);
    }

    [Fact]
    public void SinglePaletteWithDefaultValues()
    {
        var effect = SingleEffect<PaletteEffect>("<p>my text</p>", "my text");
        Assert.Single(effect.Colors);
        Assert.Equal(Color.White, effect.Colors[0]);
        Assert.Equal(0.075f, effect.Interval, precision: 4);
    }

    [Fact]
    public void SingleShadow()
    {
        var effect = SingleEffect<ShadowEffect>("<sd=#000000 2 3>my text</sd>", "my text");
        Assert.Equal(Color.Black, effect.Color);
        Assert.Equal(2f, effect.X, precision: 4);
        Assert.Equal(3f, effect.Y, precision: 4);
    }

    [Fact]
    public void SingleShadowWithDefaultValues()
    {
        var effect = SingleEffect<ShadowEffect>("<sd>my text</sd>", "my text");
        Assert.Equal(Color.Gray, effect.Color);
        Assert.Equal(-1f, effect.X, precision: 4);
        Assert.Equal(1f, effect.Y, precision: 4);
    }

    [Fact]
    public void SingleShake()
    {
        var effect = SingleEffect<ShakeEffect>("<sk=0.1 1.5>my text</sk>", "my text");
        Assert.Equal(0.1f, effect.Interval, precision: 4);
        Assert.Equal(1.5f, effect.Strength, precision: 4);
    }

    [Fact]
    public void SingleShakeWithDefaultValues()
    {
        var effect = SingleEffect<ShakeEffect>("<sk>my text</sk>", "my text");
        Assert.Equal(0.06f, effect.Interval, precision: 4);
        Assert.Equal(0.75f, effect.Strength, precision: 4);
    }

    [Fact]
    public void SingleStrikethrough()
    {
        SingleEffect<StrikethroughEffect>("<s>my text</s>", "my text");
    }

    [Fact]
    public void SingleTransitionColorModeIn()
    {
        var effect = SingleEffect<TransitionColorEffect>("<tc=#FF0000 i>my text</tc>", "my text");
        Assert.Equal(Color.Red, effect.Color);
        Assert.Equal(TransitionMode.In, effect.Mode);
    }

    [Fact]
    public void SingleTransitionColorModeOut()
    {
        var effect = SingleEffect<TransitionColorEffect>("<tc=#FF0000 o>my text</tc>", "my text");
        Assert.Equal(TransitionMode.Out, effect.Mode);
    }

    [Fact]
    public void SingleTransitionColorModeBoth()
    {
        var effect = SingleEffect<TransitionColorEffect>("<tc=#FF0000 b>my text</tc>", "my text");
        Assert.Equal(TransitionMode.Both, effect.Mode);
    }

    [Fact]
    public void SingleTransitionColorWithDefaultValues()
    {
        var effect = SingleEffect<TransitionColorEffect>("<tc>my text</tc>", "my text");
        Assert.Equal(Color.Transparent, effect.Color);
        Assert.Equal(TransitionMode.In, effect.Mode);
    }

    [Fact]
    public void SingleTransitionInterval()
    {
        var effect = SingleEffect<TransitionIntervalEffect>("<ti=0.1 o>my text</ti>", "my text");
        Assert.Equal(0.1f, effect.Interval, precision: 4);
        Assert.Equal(TransitionMode.Out, effect.Mode);
    }

    [Fact]
    public void SingleTransitionIntervalWithDefaultValues()
    {
        var effect = SingleEffect<TransitionIntervalEffect>("<ti>my text</ti>", "my text");
        Assert.Equal(0.05f, effect.Interval, precision: 4);
        Assert.Equal(TransitionMode.In, effect.Mode);
    }

    [Fact]
    public void SingleTransitionOffset()
    {
        var effect = SingleEffect<TransitionOffsetEffect>("<to=3 4 b>my text</to>", "my text");
        Assert.Equal(3f, effect.X, precision: 4);
        Assert.Equal(4f, effect.Y, precision: 4);
        Assert.Equal(TransitionMode.Both, effect.Mode);
    }

    [Fact]
    public void SingleTransitionOffsetWithDefaultValues()
    {
        var effect = SingleEffect<TransitionOffsetEffect>("<to>my text</to>", "my text");
        Assert.Equal(0f, effect.X, precision: 4);
        Assert.Equal(1f, effect.Y, precision: 4);
        Assert.Equal(TransitionMode.In, effect.Mode);
    }

    [Fact]
    public void SingleTransitionPause()
    {
        var effect = SingleEffect<TransitionPauseEffect>("<tp=2.5 o>");
        Assert.Equal(2.5f, effect.Duration, precision: 4);
        Assert.Equal(TransitionMode.Out, effect.Mode);
    }

    [Fact]
    public void SingleTransitionPauseWithDefaultValues()
    {
        var effect = SingleEffect<TransitionPauseEffect>("<tp>");
        Assert.Equal(1f, effect.Duration, precision: 4);
        Assert.Equal(TransitionMode.In, effect.Mode);
    }

    [Fact]
    public void SingleTransitionScopeLetter()
    {
        var effect = SingleEffect<TransitionScopeEffect>("<ts=lt>my text</ts>", "my text");
        Assert.Equal(TransitionScope.Letter, effect.Scope);
    }

    [Fact]
    public void SingleTransitionScopeWord()
    {
        var effect = SingleEffect<TransitionScopeEffect>("<ts=w>my text</ts>", "my text");
        Assert.Equal(TransitionScope.Word, effect.Scope);
    }

    [Fact]
    public void SingleTransitionScopeLine()
    {
        var effect = SingleEffect<TransitionScopeEffect>("<ts=lg>my text</ts>", "my text");
        Assert.Equal(TransitionScope.Line, effect.Scope);
    }

    [Fact]
    public void SingleTransitionScopeText()
    {
        var effect = SingleEffect<TransitionScopeEffect>("<ts=t>my text</ts>", "my text");
        Assert.Equal(TransitionScope.Text, effect.Scope);
    }

    [Fact]
    public void SingleTransitionScopeWithMode()
    {
        var effect = SingleEffect<TransitionScopeEffect>("<ts=w b>my text</ts>", "my text");
        Assert.Equal(TransitionScope.Word, effect.Scope);
        Assert.Equal(TransitionMode.Both, effect.Mode);
    }

    [Fact]
    public void SingleTransitionScopeWithDefaultValues()
    {
        var effect = SingleEffect<TransitionScopeEffect>("<ts>my text</ts>", "my text");
        Assert.Equal(TransitionScope.Letter, effect.Scope);
        Assert.Equal(TransitionMode.In, effect.Mode);
    }

    [Fact]
    public void SingleUnderline()
    {
        SingleEffect<UnderlineEffect>("<u>my text</u>", "my text");
    }

    [Fact]
    public void SingleWaveVertical()
    {
        var effect = SingleEffect<WaveEffect>("<w=1.1 2 3 v>my text</w>", "my text");
        Assert.Equal(1.1f, effect.Frequency, precision: 4);
        Assert.Equal(2f, effect.Amplitude, precision: 4);
        Assert.Equal(3f, effect.Phase, precision: 4);
        Assert.Equal(WaveMode.Vertical, effect.Mode);
    }

    [Fact]
    public void SingleWaveHorizontal()
    {
        var effect = SingleEffect<WaveEffect>("<w=1 2 3 h>my text</w>", "my text");
        Assert.Equal(WaveMode.Horizontal, effect.Mode);
    }

    [Fact]
    public void SingleWaveWithDefaultValues()
    {
        var effect = SingleEffect<WaveEffect>("<w>my text</w>", "my text");
        Assert.Equal(8f, effect.Frequency, precision: 4);
        Assert.Equal(2f, effect.Amplitude, precision: 4);
        Assert.Equal(1f, effect.Phase, precision: 4);
        Assert.Equal(WaveMode.Vertical, effect.Mode);
    }

    #endregion
}