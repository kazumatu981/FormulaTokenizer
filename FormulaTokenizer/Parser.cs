using FormulaTokenizer.Exceptions;
using FormulaTokenizer.Model;
namespace FormulaTokenizer;

public enum ParseState
{
    S0, S1, S2
}
public class Parser : MapReduceStateMachineBase<ParseState, ParseTree, Token, Token>
{

    #region Public Member

    #region Constructors
    public Parser() : base(ParseState.S0)
    {
    }
    #endregion

    #region Properties
    #endregion
    #region Methods
    public override void Uninitialize()
    {
        if (generator.HasCash) throw new UnexpectedTokenException();
    }
    #endregion
    #endregion

    #region Private Members
    private readonly BlanchGenerator generator = new();
    protected override Token? ElementMap(Token token)
        => (State, token) switch
        {
            (ParseState.S0, NumberToken t) => generator.GenerateBlanch(t),
            (ParseState.S0, OperatorToken t) when t.IsPlusMinus => generator.SetSign(t),

            (ParseState.S1, OperatorToken t) when t.IsPlusMinus => generator.SetOperator(t),
            (ParseState.S1, OperatorToken t) => generator.SetOperator(t),

            (ParseState.S2, NumberToken t) => generator.GenerateBlanch(t),

            _ => throw new UnexpectedTokenException()
        };

    protected override ParseTree? ElementReduce(ParseTree? previousResult, Token nextElement)
        => previousResult?.AppendBlanch(nextElement);
    protected override ParseState GetNextState(Token token)
        => (State, token) switch
        {
            (ParseState.S0, NumberToken) => ParseState.S1,
            (ParseState.S0, OperatorToken t) when t.IsPlusMinus => ParseState.S2,

            (ParseState.S1, OperatorToken t) when t.IsPlusMinus => ParseState.S0,
            (ParseState.S1, OperatorToken) => ParseState.S0,

            (ParseState.S2, NumberToken) => ParseState.S1,

            _ => throw new UnexpectedTokenException()
        };
    #endregion

    #region [Define Of Sub-class]: BlanchGenerator
    private class BlanchGenerator
    {
        private OperatorToken? Sign;
        private OperatorToken? Operator;
        public bool HasCash => Sign != null || Operator != null;
        public Token? SetSign(OperatorToken? token)
        {
            Sign = token;
            return null;
        }
        public Token? SetOperator(OperatorToken? token)
        {
            Operator = token;
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
            if (Sign != null)
            {
                Sign.RightHand = numberToken;
                blanch = Sign;
            }
            if (Operator != null)
            {
                Operator.RightHand = blanch;
                blanch = Operator;
            }
            Clear();
            return blanch;
        }
    }
    #endregion
}