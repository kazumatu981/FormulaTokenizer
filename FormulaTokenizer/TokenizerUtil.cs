namespace FormulaTokenizer;

public static class TokenizerUtil
{
    public static IEnumerable<Token> Tokenize(this IEnumerable<char> characters)
        => (new Tokenizer()).Tokenize(characters);
}