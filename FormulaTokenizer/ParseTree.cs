namespace FormulaTokenizer;

public enum ParseState
{
    S0, S1, S2
}
public class ParseTree
{

    #region Public Member

    #region Constructors
    public ParseTree(IEnumerable<Token> tokens)
    {
        builder = new TreeBuilder();
        Parse(tokens);
    }
    #endregion

    #region Properties
    public int? Result => builder.RootToken?.GetValue();
    #endregion
    #region Methods
    #endregion
    #endregion

    #region Private Members
    private ParseState CurrentState = ParseState.S0;
    private TreeBuilder builder = new();
    private void Parse(IEnumerable<Token> tokens)
    {
        foreach (Token token in tokens)
        {
            builder = RebuildTree(token);
            CurrentState = GetNextState(token);
        }
    }
    private TreeBuilder RebuildTree(Token token)
        => (CurrentState, token) switch
        {
            (ParseState.S0, NumberToken t) => builder.AppdendToTree(t).Clear(),
            (ParseState.S0, OperatorToken t) when t.IsPlusMinus => builder.SetSign(t),

            (ParseState.S1, OperatorToken t) when t.IsPlusMinus => builder.SetOperator(t),
            (ParseState.S1, OperatorToken t) => builder.SetOperator(t),

            (ParseState.S2, NumberToken t) => builder.AppdendToTree(t).Clear(),

            _ => throw new NotImplementedException()
        };
    private ParseState GetNextState(Token token)
        => (CurrentState, token) switch
        {
            (ParseState.S0, NumberToken) => ParseState.S1,
            (ParseState.S0, OperatorToken t) when t.IsPlusMinus => ParseState.S2,

            (ParseState.S1, OperatorToken t) when t.IsPlusMinus => ParseState.S0,
            (ParseState.S1, OperatorToken) => ParseState.S0,

            (ParseState.S2, NumberToken) => ParseState.S1,

            _ => throw new NotImplementedException()
        };
    #endregion

    #region [Define Of Sub-class]: TreeBuilder
    private class TreeBuilder
    {
        public Token? RootToken;
        private OperatorToken? Sign;
        private OperatorToken? Operator;
        public TreeBuilder AppdendToTree(NumberToken numberToken)
        {
            Token toBeAppend = GenerateBlanch(numberToken);

            return (RootToken, Operator) switch
            {
                (null, _) => SetRoot(toBeAppend),
                (NumberToken, _) => AppendToRoot(toBeAppend),
                (OperatorToken, OperatorToken op) when op.IsPlusMinus => AppendToRoot(toBeAppend),
                (OperatorToken, _) => SwapRootRightHand(toBeAppend),
                _ => throw new NotImplementedException()
            };
        }
        public TreeBuilder SetSign(OperatorToken? token)
        {
            Sign = token;
            return this;
        }
        public TreeBuilder SetOperator(OperatorToken? token)
        {
            Operator = token;
            return this;
        }
        public TreeBuilder Clear()
        {
            _ = SetSign(null);
            _ = SetOperator(null);
            return this;
        }
        private Token GenerateBlanch(NumberToken numberToken)
        {
            Token blanch = numberToken;
            if (Sign != null)
            {
                Sign.RightHand = numberToken;
                blanch = Sign;
            }
            return blanch;
        }
        private TreeBuilder SetRoot(Token token)
        {
            RootToken = token;
            return this;
        }
        private TreeBuilder AppendToRoot(Token token)
        {
            if (Operator != null)
            {
                Operator.LeftHand = RootToken;
                Operator.RightHand = token;
                RootToken = Operator;
            }
            else
            {
                throw new NotImplementedException();
            }
            return this;
        }
        private TreeBuilder SwapRootRightHand(Token token)
        {
            if (Operator != null && RootToken is OperatorToken currentRootOperator)
            {
                Operator.LeftHand = currentRootOperator.RightHand;
                Operator.RightHand = token;
                currentRootOperator.RightHand = Operator;
            }
            else
            {
                throw new NotImplementedException();
            }
            return this;
        }
    }
    #endregion
}