// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

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
    #endregion

    public override IEnumerable<Token> Map(IEnumerable<Token> elements)
    {
        var mapResults = base.Map(elements);
        foreach (var result in mapResults)
        {
            yield return result;
        }
        if (_generator.HasCash) throw new UnexpectedTokenException();
    }

    #region Private Members
    private readonly ParseTreeBlanchGenerator _generator = new();
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
    protected override ParseTree? ElementReduce(ParseTree? previousResult, Token nextElement)
        => previousResult?.AppendBlanch(nextElement);
    #endregion

}