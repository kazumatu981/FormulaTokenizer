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
    public Token? RootToken;
    #endregion
    #region Methods
    public ParseTree AppendBlanch(Token blanch)
    => (RootToken, blanch) switch
    {
        (null, _)
            => SetToRoot(blanch),
        (_, OperatorToken operatorToken) when operatorToken.IsPlusMinus
            => SwapRoot(operatorToken),
        (NumberToken, OperatorToken operatorBlanch)
            => SwapRoot(operatorBlanch),
        (OperatorToken rootOperatorToken, OperatorToken operatorBlanch)
            => SwapRightHandOfRoot(rootOperatorToken, operatorBlanch),
        _ => throw new UnexpectedTokenException()
    };
    #endregion
    #endregion

    #region Private Member
    private ParseTree SetToRoot(Token blanch)
    {
        RootToken = blanch;
        return this;
    }
    private ParseTree SwapRoot(OperatorToken operatorBlanch)
    {
        operatorBlanch.LeftHand = RootToken;
        RootToken = operatorBlanch;
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