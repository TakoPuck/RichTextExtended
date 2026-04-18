namespace RichTextExtended.Source.Tokenizer;

public class CloseTagToken : IToken
{
    public string Name { get; }


    public CloseTagToken(string name)
    {
        Name = name;
    }

    public override string ToString()
    {
        return $"Name: '{Name}'";
    }
}
