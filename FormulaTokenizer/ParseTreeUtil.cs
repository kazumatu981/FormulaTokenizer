namespace FormulaTokenizer;

public static class ParseTreeUtil
{
    public static ParseTree Parse(this IEnumerable<Token> tokenized) => new(tokenized);
}