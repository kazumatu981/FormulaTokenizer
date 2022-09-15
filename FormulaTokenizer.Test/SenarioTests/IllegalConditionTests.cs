using FormulaTokenizer;
using FormulaTokenizer.Exceptions;
using Xunit;

namespace FormulaTokenizer.Test.SenarioTests;

public class IllegalConditionTests
{
    // 不明な文字エラー
    [Theory]
    [InlineData("123b456")]
    [InlineData("123 b 456")]
    [InlineData("123 % 456")]
    [InlineData("123 +　 456")]
    [InlineData("123 +あ 456")]
    public void UnexpectedCharaExceptionCasesTest(string formula)
    {
        Assert.Throws<UnexpectedCharException>(() =>
        {
            _ = formula.GetChars().Tokenize().Parse()?.RootToken?.GetValue();
        });
    }

    // 不明なトークンエラー
    [Theory]
    [InlineData("+")]
    [InlineData("++123")]
    [InlineData("123++")]
    [InlineData("123+*1")]
    [InlineData("123+-")]
    [InlineData("123**123")]
    [InlineData("123    456")]
    [InlineData("123     456")]
    public void UnexpectedTokenExceptionCasesTest(string formula)
    {
        Assert.Throws<UnexpectedTokenException>(() =>
        {
            var tree = formula
                .GetChars()
                .Tokenize()
                .Parse()?
                .RootToken?
                .GetValue();
        });
    }
}