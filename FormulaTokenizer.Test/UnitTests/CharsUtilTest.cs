using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace FormulaTokenizer.Test;

public class CharsUtilTest
{
    [Fact]
    public void GetChars_NoArg()
    {
        var test = "abc";
        var expected = new[] { 'a', 'b', 'c', '\0' };
        var actual = test.GetChars();
        Assert.Equal(expected, actual);
    }
    [Fact]
    public void GetChars_Arg_false()
    {
        var test = "abc";
        var expected = new[] { 'a', 'b', 'c' };
        var actual = test.GetChars(false);
        Assert.Equal(expected, actual);
    }
    [Fact]
    public void GetChars_Arg_true()
    {
        var test = "abc";
        var expected = new[] { 'a', 'b', 'c', '\0' };
        var actual = test.GetChars(true);
        Assert.Equal(expected, actual);
    }
    [Fact]
    public void GetChars_EmptyString()
    {
        var test = string.Empty;
        var expected = new[] { '\0' };
        var actual = test.GetChars();
        Assert.Equal(expected, actual);
    }
    [Fact]
    public void GetChars_EmptyString_Arg_false()
    {
        var test = string.Empty;
        var expected = Enumerable.Empty<char>();
        var actual = test.GetChars(false);
        Assert.Equal(expected, actual);
    }
}