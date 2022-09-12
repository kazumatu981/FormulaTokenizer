using FormulaTokenizer;
using FormulaTokenizer.Exceptions;
using Xunit;

namespace FormulaTokenizer.Test;

public class NumberTokenTest
{
    [Fact]
    public void TokenTypeIsNumber()
    {
        var expected = TokenType.Number;
        var actual = new NumberToken("1").Type;

        Assert.Equal(expected, actual);
    }
    [Theory]
    [InlineData("123", 123)]
    [InlineData("103", 103)]
    [InlineData("225", 225)]
    [InlineData("65535", 65535)]
    public void GetValueTest(string test, int expected)
    {
        var numberToken = new NumberToken(test);
        var actual = numberToken.GetValue();

        Assert.Equal(expected, actual);
    }
    [Theory]
    [InlineData("12a3")]
    [InlineData("12+3")]
    [InlineData("(123")]
    [InlineData("123  ")]
    [InlineData("123@")]
    [InlineData("1^23")]
    public void UnexpectedChar(string test)
    {
        Assert.Throws<UnexpectedCharException>(() =>
        {
            _ = new NumberToken(test);
        });
    }
}