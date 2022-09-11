namespace FormulaTokenizer;

public enum ParseState
{
    S0, S1, S2
}
public class ParseTree
{
    private Token? RootToken;

    private ParseState CurrentState = ParseState.S0;
    private TreeBuilder builder = new();

    private class TreeBuilder
    {
        public Token? RootToken;
        public OperatorToken? Sign;
        public OperatorToken? Operator;

        public TreeBuilder AppdendToTree(NumberToken numberToken)
        {
            Token toBeAppend = numberToken;
            if (Sign != null)
            {
                Sign.RightHand = numberToken;
                toBeAppend = Sign;
            }

            if (RootToken == null)
            {
                RootToken = toBeAppend;
            }
            else if (RootToken is NumberToken)
            {
                if (Operator != null)
                {
                    Operator.LeftHand = RootToken;
                    Operator.RightHand = toBeAppend;
                    RootToken = Operator;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            else if (RootToken is OperatorToken currentRootOperator)
            {
                if (Operator != null)
                {
                    Operator.LeftHand = currentRootOperator.RightHand;
                    Operator.RightHand = toBeAppend;
                    currentRootOperator.RightHand = Operator;
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
            return this;
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
    }

    public Token? Parse(IEnumerable<Token> tokens)
    {
        foreach (Token token in tokens)
        {
            var nextState = GetNextState(token);
            _ = ChangeTree(token);

            CurrentState = nextState;

        }
        return builder.RootToken;
    }

    private ParseState GetNextState(Token token)
    {
        return (CurrentState, token) switch
        {
            (ParseState.S0, NumberToken) => ParseState.S1,
            (ParseState.S0, OperatorToken t) when t.IsPlusMinus => ParseState.S2,
            (ParseState.S0, OperatorToken) => throw new NotImplementedException(),

            (ParseState.S1, NumberToken) => throw new NotImplementedException(),
            (ParseState.S1, OperatorToken t) when t.IsPlusMinus => ParseState.S0,
            (ParseState.S1, OperatorToken) => ParseState.S0,

            (ParseState.S2, NumberToken) => ParseState.S1,

            _ => throw new NotImplementedException()
        };
    }

    private TreeBuilder ChangeTree(Token token)
    {
        return (CurrentState, token) switch
        {
            (ParseState.S0, NumberToken t) => builder.AppdendToTree(t).SetSign(null).SetOperator(null),
            (ParseState.S0, OperatorToken t) when t.IsPlusMinus => builder.SetSign(t),
            (ParseState.S0, OperatorToken) => throw new NotImplementedException(),

            (ParseState.S1, NumberToken) => throw new NotImplementedException(),
            (ParseState.S1, OperatorToken t) when t.IsPlusMinus => builder.SetOperator(t),
            (ParseState.S1, OperatorToken t) => builder.SetOperator(t),

            (ParseState.S2, NumberToken t) => builder.AppdendToTree(t).SetSign(null).SetOperator(null),

            _ => throw new NotImplementedException()
        };

    }
    public int? Run() => RootToken?.GetValue();
}