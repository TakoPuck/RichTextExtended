namespace RichTextExtended.TextEffects;

public abstract class TextEffect
{
    public abstract string TagName { get; }

    public bool IsEnabled { get; set; } = true;
}
