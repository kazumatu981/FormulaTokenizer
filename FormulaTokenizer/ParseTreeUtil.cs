namespace FormulaTokenizer;

public static class ParseTreeUtil
{
    public static Token? Parse(this IEnumerable<Token> tokenized)
    {
        return (new ParseTree()).Parse(tokenized) as Token;
    }
}