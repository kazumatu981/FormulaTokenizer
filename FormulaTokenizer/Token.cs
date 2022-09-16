// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

    public abstract int GetValue();

    protected Token(TokenType type, string text)
    {
        Type = type;
        Text = text;
    }
}
