// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using FormulaTokenizer;
using Xunit;

namespace FormulaTokenizer.Test.SenarioTests;

public class BasicConditionTests
{
    [Theory]
    // 一項計算
    [InlineData("1", 1)]
    [InlineData("+2", 2)]
    [InlineData("-3", -3)]
    [InlineData(" -3", -3)]
    [InlineData("-  3", -3)]
    [InlineData(" - 3  ", -3)]
    [InlineData("-3    ", -3)]
    // 二項計算
    [InlineData("1+2", 3)]
    [InlineData("9*9", 81)]
    [InlineData("8/2", 4)]
    [InlineData("8-2", 6)]
    // 三項計算(優先度なし)
    [InlineData("8+2+4", 14)]
    [InlineData("8+2-4", 6)]
    // 三項計算(優先度あり)
    [InlineData("8+2*4", 16)]
    // 四項計算
    [InlineData("8+2*4+5", 21)]
    [InlineData("8/2+4*5", 24)]
    // マイナス
    [InlineData("-1+2", 1)]
    [InlineData("-1+-2", -3)]
    [InlineData("-1*-2", 2)]
    [InlineData("-4/2", -2)]
    // スペースあり
    [InlineData(" 1+2", 3)]
    [InlineData("9    *9", 81)]
    [InlineData("8/  2", 4)]
    [InlineData("8-2    ", 6)]
    [InlineData("-  1+2", 1)]
    [InlineData("-1+ - 2", -3)]
    public void BasicTest(string formula, int expected)
    {
        var actual = formula.GetChars().Tokenize().Parse()?.RootToken?.GetValue();
        Assert.Equal(expected, actual);
    }
}