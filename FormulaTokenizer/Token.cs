namespace FormulaTokenizer;

public enum TokenType
{
    Unknown,
    Number,
    Operator,
}
public abstract class Token
{
    public readonly TokenType Type;
    public readonly string Text;

    public Token? Parent;

    public abstract int GetValue();

    public Token(TokenType type, string text)
    {
        Type = type;
        Text = text;
    }
}
