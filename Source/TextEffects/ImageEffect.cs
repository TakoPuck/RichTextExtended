namespace RichTextExtended.Source.TextEffects;

public class ImageEffect : TextEffect
{
    public const string TAG = "img";

    public override string TagName => TAG;

    public string Id { get; set; }
}
