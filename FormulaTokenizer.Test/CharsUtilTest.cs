using Xunit;

namespace FormulaTokenizer.Test;

public class CharsUtilTest
{
    [Theory]
    [InlineData("abc", 0, 'a')]
    [InlineData("abc", -100, '\0')]
    [InlineData("abc", -1, '\0')]
    [InlineData("abc", 2, 'c')]
    [InlineData("abc", 3, '\0')]
    [InlineData("abc", 4, '\0')]
    [InlineData("abc", 10, '\0')]
    public void SafeGetAt_NormalTest(string test, int index, char expected)
    {
        char? actual = test.SafeGetAt(index);
        Assert.Equal(expected, actual);
    }
}