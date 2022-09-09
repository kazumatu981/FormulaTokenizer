using FormulaTokenizer;
using Xunit;

namespace FormulaTokenizer.Test;

public class NumberTokenTest
{
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
}