namespace FormulaTokenizer;

public static class TokenizerUtil
{
    public static IEnumerable<Token> Token(this IEnumerable<char> characters)
        => (new Tokenizer()).Tokenize(characters);
}