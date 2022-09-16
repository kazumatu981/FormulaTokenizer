// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        if (_generator.HasCash) throw new UnexpectedTokenException();
    }
    #endregion
    #endregion

    #region Private Members
    private readonly BlanchGenerator _generator = new();
    protected override Token? ElementMap(Token token)
        => (State, token) switch
        {
            (ParseState.S0, NumberToken t) => _generator.GenerateBlanch(t),
            (ParseState.S0, OperatorToken t) when t.IsPlusMinus => _generator.SetSign(t),

            (ParseState.S1, OperatorToken t) when t.IsPlusMinus => _generator.SetOperator(t),
            (ParseState.S1, OperatorToken t) => _generator.SetOperator(t),

            (ParseState.S2, NumberToken t) => _generator.GenerateBlanch(t),

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
    private sealed class BlanchGenerator
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
    #endregion
}