using System.Linq;
using FormulaTokenizer.Test.Comparer;
using Xunit;

namespace FormulaTokenizer.Test;

public class UnitTest1
{
    FormulaTokenComparer toeknComparer = new();

    [Theory]
    [InlineData("1+234")]
    [InlineData("   1+234")]
    [InlineData("1+234    ")]
    [InlineData("1  + 234")]
    [InlineData(" 1 + 234 ")]
    [InlineData("1 +234")]
    [InlineData("1+234 ")]
    public void Test001(string test)
    {
        var expected = new Token[]
        {
            new NumberToken( "1"),
            new OperatorToken( "+"),
            new NumberToken( "234"),
        };
        var tokenizer = new Tokenizer();

        var actual = tokenizer.Tokenize(test.GetChars());

        Assert.Equal(expected, actual, toeknComparer);
    }
    [Theory]
    [InlineData("111+0*124")]
    [InlineData("111+0*124 ")]
    [InlineData("111+0* 124")]
    [InlineData("111+ 0*124")]
    [InlineData("111+0    *124")]
    [InlineData(" 111 +0*124")]
    public void Test002(string test)
    {
        var expected = new Token[]
        {
            new NumberToken( "111"),
            new OperatorToken ( "+"),
            new NumberToken(  "0"),
            new OperatorToken( "*"),
            new NumberToken(  "124"),
        };
        var tokenizer = new Tokenizer();

        var actual = tokenizer.Tokenize(test.GetChars());

        Assert.Equal(expected, actual, toeknComparer);
    }

    [Theory]
    [InlineData("0+-124")]
    [InlineData("0+  -124 ")]
    [InlineData("0+  - 124 ")]
    [InlineData(" 0+  -124 ")]
    [InlineData("0 +  -124 ")]
    [InlineData("0 +- 124 ")]
    public void Test003(string test)
    {
        var expected = new Token[]
        {
            new NumberToken( "0"),
            new OperatorToken("+"),
            new OperatorToken( "-"),
            new NumberToken( "124"),
        };
        var tokenizer = new Tokenizer();

        var actual = tokenizer.Tokenize(test.GetChars());

        Assert.Equal(expected, actual, toeknComparer);
    }

}