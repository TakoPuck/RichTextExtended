namespace RichTextExtended.Source.Banks;

public class BankRegistry
{
    private static BankRegistry _instance;


    public static BankRegistry Instance
    {
        get
        {
            _instance ??= new();
            return _instance;
        }
    }

    public PaletteBank PaletteBank { get; }

    public ImageBank ImageBank { get; }

    public ColorBank ColorBank { get; }

    public FontBank FontBank { get; }


    private BankRegistry()
    {
        PaletteBank = new();
        ColorBank = new();
        ImageBank = new();
        FontBank = new();
    }
}
