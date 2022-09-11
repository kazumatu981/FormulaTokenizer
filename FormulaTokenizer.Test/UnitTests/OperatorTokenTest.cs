using FormulaTokenizer;
using Xunit;

namespace FormulaTokenizer.Test;

public class OperatorTokenTest
{
    [Fact]
    public void SumOperator()
    {
        var x = new NumberToken("123");
        var y = new NumberToken("555");

        var addOperator = new OperatorToken("+")
        {
            LeftHand = x,
            RightHand = y
        };

        var expected = 678;
        var actual = addOperator.GetValue();

        Assert.Equal(expected, actual);
    }
    [Fact]
    public void SubstructOperator()
    {
        var x = new NumberToken("753");
        var y = new NumberToken("555");

        var addOperator = new OperatorToken("-")
        {
            LeftHand = x,
            RightHand = y
        };

        var expected = 198;
        var actual = addOperator.GetValue();

        Assert.Equal(expected, actual);
    }
}