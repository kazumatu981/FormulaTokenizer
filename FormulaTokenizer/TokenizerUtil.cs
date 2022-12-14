// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace FormulaTokenizer;

public enum CharType
{
    Unknown,
    WhiteSpace,
    EOL,
    Operator,
    NonZeroNumber,
    Zero,
}

public static class TokenizerUtil
{
    public static IEnumerable<Token> Tokenize(this IEnumerable<char> characters)
        => (new Tokenizer()).Map(characters);
    public static CharType GetCharType(this char thisChar)
    {
        var charType = thisChar switch
        {
            char c when c.IsWhiteSpace() => CharType.WhiteSpace,
            char c when c.IsEol() => CharType.EOL,
            char c when c.IsOperator() => CharType.Operator,
            char c when c.IsNonZeroNumber() => CharType.NonZeroNumber,
            char c when c.IsZero() => CharType.Zero,
            _ => CharType.Unknown
        };

        return charType;
    }

    #region private methods
    private static bool IsWhiteSpace(this char thisChar)
        => ' ' == thisChar;
    private static bool IsEol(this char thisChar)
        => '\0' == thisChar;
    private static bool IsOperator(this char thisChar)
        => '+' == thisChar || '-' == thisChar || '*' == thisChar || '/' == thisChar;

    private static bool IsNonZeroNumber(this char thisChar)
        => '1' <= thisChar && thisChar <= '9';

    private static bool IsZero(this char thisChar)
        => '0' == thisChar;
    #endregion

}