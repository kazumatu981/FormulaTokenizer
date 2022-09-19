// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using FormulaTokenizer.Exceptions;

namespace FormulaTokenizer;

public class ParseTree
{

    #region Public Member

    #region Constructors
    public ParseTree()
    {
    }
    #endregion

    #region Properties
    private Token? _rootToken;
    public Token? RootToken => _rootToken;
    #endregion
    #region Methods
    public ParseTree AppendBlanch(Token blanch)
    => (_rootToken, blanch) switch
    {
        // RootToken is null
        (null, OperatorToken) => SetToRoot(blanch),
        (null, NumberToken) => SetToRoot(blanch),

        // RootToken is NumberToken
        (NumberToken, OperatorToken blanchOperator) => SwapRoot(blanchOperator),

        // RootToken is OperatorToken
        (OperatorToken, OperatorToken blanchOperator) when blanchOperator.IsPlusMinus
            => SwapRoot(blanchOperator),
        (OperatorToken rootOperator, OperatorToken blanchOperator)
            => SwapRightHandOfRoot(rootOperator, blanchOperator),

        // Others are Unexpected
        _ => throw new UnexpectedTokenException()
    };
    #endregion
    #endregion

    #region Private Member
    private ParseTree SetToRoot(Token blanch)
    {
        _rootToken = blanch;
        return this;
    }
    private ParseTree SwapRoot(OperatorToken operatorBlanch)
    {
        operatorBlanch.LeftHand = _rootToken;
        _rootToken = operatorBlanch;
        return this;
    }
    private ParseTree SwapRightHandOfRoot(
        OperatorToken rootOperatorToken,
        OperatorToken operatorBlanch)
    {
        operatorBlanch.LeftHand = rootOperatorToken.RightHand;
        rootOperatorToken.RightHand = operatorBlanch;
        return this;
    }
    #endregion
}