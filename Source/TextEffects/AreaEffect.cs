namespace RichTextExtended.Source.TextEffects;

public class AreaEffect : TextEffect
{
    public const string TAG = "a";

    public override string TagName => TAG;

    public string Id { get; set; }
}
