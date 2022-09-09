using System.Collections.Generic;
using System.Text;

namespace FormulaTokenizer;

public enum TokenizeState
{
    S0,
    Num0,
    Num1,
    Op
}
public class Tokenizer
{
    #region [Private Sub-class]: TokenGenerator
    private class TokenGenerator
    {
        readonly StringBuilder builder = new();
        private TokenType CurrentTokenType = TokenType.Unknown;

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

    private TokenizeState currentState = TokenizeState.S0;
    private readonly TokenGenerator generator = new();
    public IEnumerable<Token> Tokenize(IEnumerable<char> formulaChars)
        => formulaChars
            .Select((c) =>
            {
                var token = ReadToken(c);
                currentState = NextState(c);
                return token;
            })
            .Where(token => token != null)
            .Cast<Token>();

    private Token? ReadToken(char character)
        => (currentState, character.GetCharType()) switch
        {
            (TokenizeState.S0, CharType.Zero) => generator.NewToken(TokenType.Number, character),
            (TokenizeState.S0, CharType.NonZeroNumber) => generator.NewToken(TokenType.Number, character),
            (TokenizeState.S0, CharType.Operator) => generator.NewToken(TokenType.Operator, character),
            (TokenizeState.S0, CharType.WhiteSpace) => null,
            (TokenizeState.S0, CharType.EOL) => null,

            (TokenizeState.Num0, CharType.Zero) => throw new InvalidDataException(),
            (TokenizeState.Num0, CharType.NonZeroNumber) => throw new InvalidDataException(),
            (TokenizeState.Num0, CharType.Operator) => generator.DrainAndNewToken(TokenType.Operator, character),
            (TokenizeState.Num0, CharType.WhiteSpace) => generator.Drain(),
            (TokenizeState.Num0, CharType.EOL) => generator.Drain(),

            (TokenizeState.Num1, CharType.Zero) => generator.Keep(character),
            (TokenizeState.Num1, CharType.NonZeroNumber) => generator.Keep(character),
            (TokenizeState.Num1, CharType.Operator) => generator.DrainAndNewToken(TokenType.Operator, character),
            (TokenizeState.Num1, CharType.WhiteSpace) => generator.Drain(),
            (TokenizeState.Num1, CharType.EOL) => generator.Drain(),

            (TokenizeState.Op, CharType.Zero) => generator.DrainAndNewToken(TokenType.Number, character),
            (TokenizeState.Op, CharType.NonZeroNumber) => generator.DrainAndNewToken(TokenType.Number, character),
            (TokenizeState.Op, CharType.Operator) => generator.DrainAndNewToken(TokenType.Operator, character),
            (TokenizeState.Op, CharType.WhiteSpace) => generator.Drain(),
            (TokenizeState.Op, CharType.EOL) => generator.Drain(),

            _ => throw new NotImplementedException()
        };

    private TokenizeState NextState(char character)
        => (currentState, character.GetCharType()) switch
        {
            (TokenizeState.S0, CharType.Zero) => TokenizeState.Num0,
            (TokenizeState.S0, CharType.NonZeroNumber) => TokenizeState.Num1,
            (TokenizeState.S0, CharType.Operator) => TokenizeState.Op,
            (TokenizeState.S0, CharType.WhiteSpace) => TokenizeState.S0,
            (TokenizeState.S0, CharType.EOL) => TokenizeState.S0,

            (TokenizeState.Num0, CharType.Zero) => throw new InvalidDataException(),
            (TokenizeState.Num0, CharType.NonZeroNumber) => throw new InvalidDataException(),
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

}