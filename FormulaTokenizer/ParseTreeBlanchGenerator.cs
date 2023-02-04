// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using FormulaTokenizer.Exceptions;

namespace FormulaTokenizer;

sealed class ParseTreeBlanchGenerator
{
    private OperatorToken? _signToken;
    private OperatorToken? _operatorToken;
    public bool HasCash => _signToken != null || _operatorToken != null;
    public Token? SetSign(OperatorToken? token)
    {
        _signToken = token;
        return null;
    }
    public Token? SetOperator(OperatorToken? token)
    {
        _operatorToken = token;
        return null;
    }
    private void Clear()
    {
        _ = SetSign(null);
        _ = SetOperator(null);
    }
    public Token GenerateBlanch(NumberToken numberToken)
    {
        Token blanch = numberToken;
        if (_signToken != null)
        {
            _signToken.RightHand = numberToken;
            blanch = _signToken;
        }
        if (_operatorToken != null)
        {
            _operatorToken.RightHand = blanch;
            blanch = _operatorToken;
        }
        Clear();
        return blanch;
    }
}
