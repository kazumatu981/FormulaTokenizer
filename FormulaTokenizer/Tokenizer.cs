// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;
using FormulaTokenizer.Exceptions;
using FormulaTokenizer.Model;

namespace FormulaTokenizer;

public enum TokenizeState
{
    S0, Num0, Num1, Op
}
public class Tokenizer : MapStateMachineBase<TokenizeState, Token, char>
{
    private readonly TokenGenerator _generator = new();

    #region Public Members
    #region Constructors
    public Tokenizer(TokenizeState initialState = TokenizeState.S0) : base(initialState)
    {
    }
    #endregion

    #region Properties
    #endregion
    #region Methods
    public override void Reset()
    {
        base.Reset();
        _generator.Clean();
    }
    protected override Token? ElementMap(char element)
        => (State, element.GetCharType()) switch
        {
            (TokenizeState.S0, CharType.Zero) => _generator.NewToken(TokenType.Number, element),
            (TokenizeState.S0, CharType.NonZeroNumber) => _generator.NewToken(TokenType.Number, element),
            (TokenizeState.S0, CharType.Operator) => _generator.NewToken(TokenType.Operator, element),
            (TokenizeState.S0, CharType.WhiteSpace) => null,
            (TokenizeState.S0, CharType.EOL) => null,

            (TokenizeState.Num0, CharType.Operator) => _generator.DrainAndNewToken(TokenType.Operator, element),
            (TokenizeState.Num0, CharType.WhiteSpace) => _generator.Drain(),
            (TokenizeState.Num0, CharType.EOL) => _generator.Drain(),

            (TokenizeState.Num1, CharType.Zero) => _generator.Keep(element),
            (TokenizeState.Num1, CharType.NonZeroNumber) => _generator.Keep(element),
            (TokenizeState.Num1, CharType.Operator) => _generator.DrainAndNewToken(TokenType.Operator, element),
            (TokenizeState.Num1, CharType.WhiteSpace) => _generator.Drain(),
            (TokenizeState.Num1, CharType.EOL) => _generator.Drain(),

            (TokenizeState.Op, CharType.Zero) => _generator.DrainAndNewToken(TokenType.Number, element),
            (TokenizeState.Op, CharType.NonZeroNumber) => _generator.DrainAndNewToken(TokenType.Number, element),
            (TokenizeState.Op, CharType.Operator) => _generator.DrainAndNewToken(TokenType.Operator, element),
            (TokenizeState.Op, CharType.WhiteSpace) => _generator.Drain(),
            (TokenizeState.Op, CharType.EOL) => _generator.Drain(),

            _ => throw new UnexpectedCharException()
        };
    protected override TokenizeState GetNextState(char character)
        => (State, character.GetCharType()) switch
        {
            (TokenizeState.S0, CharType.Zero) => TokenizeState.Num0,
            (TokenizeState.S0, CharType.NonZeroNumber) => TokenizeState.Num1,
            (TokenizeState.S0, CharType.Operator) => TokenizeState.Op,
            (TokenizeState.S0, CharType.WhiteSpace) => TokenizeState.S0,
            (TokenizeState.S0, CharType.EOL) => TokenizeState.S0,

            (TokenizeState.Num0, CharType.Operator) => TokenizeState.Op,
            (TokenizeState.Num0, CharType.WhiteSpace) => TokenizeState.S0,
            (TokenizeState.Num0, CharType.EOL) => TokenizeState.S0,

            (TokenizeState.Num1, CharType.Zero) => TokenizeState.Num1,
            (TokenizeState.Num1, CharType.NonZeroNumber) => TokenizeState.Num1,
            (TokenizeState.Num1, CharType.Operator) => TokenizeState.Op,
            (TokenizeState.Num1, CharType.WhiteSpace) => TokenizeState.S0,
            (TokenizeState.Num1, CharType.EOL) => TokenizeState.S0,

            (TokenizeState.Op, CharType.Zero) => TokenizeState.Num0,
            (TokenizeState.Op, CharType.NonZeroNumber) => TokenizeState.Num1,
            (TokenizeState.Op, CharType.Operator) => TokenizeState.Op,
            (TokenizeState.Op, CharType.WhiteSpace) => TokenizeState.S0,
            (TokenizeState.Op, CharType.EOL) => TokenizeState.S0,

            _ => throw new UnexpectedCharException()
        };
    #endregion
    #endregion
}