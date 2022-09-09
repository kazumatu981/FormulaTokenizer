namespace FormulaTokenizer;

public class NumberToken : Token
{
    public NumberToken(string text) : base(TokenType.Number, text) { }

    public override int GetValue()
    {
        var ret = 0;
        foreach (char c in Text.GetChars(false))
        {
            ret *= 10;
            ret += c - '0';
        }
        return ret;
    }
}