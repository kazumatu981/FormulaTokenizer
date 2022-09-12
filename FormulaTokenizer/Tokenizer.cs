using System.Collections.Generic;
using System.Text;
using FormulaTokenizer.Model;

namespace FormulaTokenizer;

public enum TokenizeState
{
    S0, Num0, Num1, Op
}
public class Tokenizer : MapStateMachineBase<TokenizeState, Token, char>
{
    private readonly TokenGenerator generator = new();

    #region Public Members
    #region Constructors
    public Tokenizer(TokenizeState initialState = TokenizeState.S0) : base(initialState)
    {
    }
    #endregion

    #region Properties
    #endregion
    #region Methods
    public override void Initialize()
    {
        base.Initialize();
        generator.Clean();
    }

    protected override Token? ElementMap(char element)
        => (State, element.GetCharType()) switch
        {
            (TokenizeState.S0, CharType.Zero) => generator.NewToken(TokenType.Number, element),
            (TokenizeState.S0, CharType.NonZeroNumber) => generator.NewToken(TokenType.Number, element),
            (TokenizeState.S0, CharType.Operator) => generator.NewToken(TokenType.Operator, element),
            (TokenizeState.S0, CharType.WhiteSpace) => null,
            (TokenizeState.S0, CharType.EOL) => null,

            (TokenizeState.Num0, CharType.Operator) => generator.DrainAndNewToken(TokenType.Operator, element),
            (TokenizeState.Num0, CharType.WhiteSpace) => generator.Drain(),
            (TokenizeState.Num0, CharType.EOL) => generator.Drain(),

            (TokenizeState.Num1, CharType.Zero) => generator.Keep(element),
            (TokenizeState.Num1, CharType.NonZeroNumber) => generator.Keep(element),
            (TokenizeState.Num1, CharType.Operator) => generator.DrainAndNewToken(TokenType.Operator, element),
            (TokenizeState.Num1, CharType.WhiteSpace) => generator.Drain(),
            (TokenizeState.Num1, CharType.EOL) => generator.Drain(),

            (TokenizeState.Op, CharType.Zero) => generator.DrainAndNewToken(TokenType.Number, element),
            (TokenizeState.Op, CharType.NonZeroNumber) => generator.DrainAndNewToken(TokenType.Number, element),
            (TokenizeState.Op, CharType.Operator) => generator.DrainAndNewToken(TokenType.Operator, element),
            (TokenizeState.Op, CharType.WhiteSpace) => generator.Drain(),
            (TokenizeState.Op, CharType.EOL) => generator.Drain(),

            _ => throw new NotImplementedException()
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

            _ => throw new NotImplementedException()
        };
    #endregion
    #endregion
    #region Private Members


    #endregion
    #region [Define Of Sub-class]: TokenGenerator
    private class TokenGenerator
    {
        readonly StringBuilder builder = new();
        private TokenType CurrentTokenType = TokenType.Unknown;

        public void Clean()
        {
            builder.Clear();
            CurrentTokenType = TokenType.Unknown;
        }

        public Token? NewToken(TokenType type, char character)
        {
            builder.Clear();
            CurrentTokenType = type;
            builder.Append(character);
            return null;
        }

        public Token? Keep(char character)
        {
            builder.Append(character);
            return null;
        }

        public Token Drain()
            => CurrentTokenType switch
            {
                TokenType.Number => new NumberToken(builder.ToString()),
                TokenType.Operator => new OperatorToken(builder.ToString()),
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
    #endregion
}