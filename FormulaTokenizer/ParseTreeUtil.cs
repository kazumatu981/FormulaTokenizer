namespace FormulaTokenizer;

public static class ParserUtil
{
    public static ParseTree? Parse(this IEnumerable<Token> tokenized)
        => (new Parser()).MapReduce(tokenized, new ParseTree());
}