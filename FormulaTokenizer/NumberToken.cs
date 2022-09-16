// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;

using FormulaTokenizer.Exceptions;

namespace FormulaTokenizer;

public class NumberToken : Token
{
    public NumberToken(string text) : base(TokenType.Number, text)
    {
        if (text.GetChars(false).Any(c => c.GetCharType() != CharType.Zero && c.GetCharType() != CharType.NonZeroNumber))
            throw new UnexpectedCharException();
    }

    public override int GetValue()
    {
        var ret = 0;
        foreach (var c in Text.GetChars(false))
        {
            ret *= 10;
            ret += c - '0';
        }
        return ret;
    }
}