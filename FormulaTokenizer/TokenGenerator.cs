// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;
using FormulaTokenizer.Exceptions;
using FormulaTokenizer.Model;

namespace FormulaTokenizer;

sealed class TokenGenerator
{
    readonly StringBuilder _builder = new();
    private TokenType _currentTokenType = TokenType.Unknown;

    public void Clean()
    {
        _builder.Clear();
        _currentTokenType = TokenType.Unknown;
    }

    public Token? NewToken(TokenType type, char character)
    {
        _builder.Clear();
        _currentTokenType = type;
        _builder.Append(character);
        return null;
    }

    public Token? Keep(char character)
    {
        _builder.Append(character);
        return null;
    }

    public Token Drain()
        => _currentTokenType switch
        {
            TokenType.Number => new NumberToken(_builder.ToString()),
            TokenType.Operator => new OperatorToken(_builder.ToString()),
            _ => throw new NotImplementedException()
        };

    public Token DrainAndNewToken(TokenType nextTokenType, char nextCharacter)
    {
        var formulaToken = Drain();
        if (nextTokenType is TokenType tokenType && nextCharacter is char character)
        {
            NewToken(tokenType, character);
        }
        return formulaToken;
    }
}
