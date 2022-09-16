// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using FormulaTokenizer;
using FormulaTokenizer.Exceptions;
using Xunit;

namespace FormulaTokenizer.Test;

public class OperatorTokenTest
{
    #region TokenType Is Operator
    [Fact]
    public void TokenTypeIsOperator()
    {
        var expected = TokenType.Operator;
        var actual = new OperatorToken("+").Type;

        Assert.Equal(expected, actual);
    }
    #endregion
    #region Kind Property Test
    [Fact]
    public void OperatorKindTest_Plus()
    {
        var test = "+";
        var expected = OperatorKind.Plus;

        var operatorToken = new OperatorToken(test);

        Assert.Equal(expected, operatorToken.Kind);
    }
    [Fact]
    public void OperatorKindTest_Minus()
    {
        var test = "-";
        var expected = OperatorKind.Minus;

        var operatorToken = new OperatorToken(test);

        Assert.Equal(expected, operatorToken.Kind);
    }
    [Fact]
    public void OperatorKindTest_Product()
    {
        var test = "*";
        var expected = OperatorKind.Product;

        var operatorToken = new OperatorToken(test);

        Assert.Equal(expected, operatorToken.Kind);
    }
    [Fact]
    public void OperatorKindTest_Devide()
    {
        var test = "/";
        var expected = OperatorKind.Devide;

        var operatorToken = new OperatorToken(test);

        Assert.Equal(expected, operatorToken.Kind);
    }
    [Fact]
    public void OperatorUnknown()
    {
        var test = "%";
        Assert.Throws<UnexpectedCharException>(() =>
        {
            _ = new OperatorToken(test);
        });
    }
    #endregion

    #region GetValue() Tests
    #region 二項間演算
    [Fact]
    public void PlusOperatorTest()
    {
        var operatorToken = new OperatorToken("+")
        {
            LeftHand = new NumberToken("123"),
            RightHand = new NumberToken("555")
        };
        var expected = 678;

        var actual = operatorToken.GetValue();

        Assert.Equal(expected, actual);
    }
    [Fact]
    public void MinusOperatorTest()
    {
        var operatorToken = new OperatorToken("-")
        {
            LeftHand = new NumberToken("123"),
            RightHand = new NumberToken("23")
        };
        var expected = 100;

        var actual = operatorToken.GetValue();

        Assert.Equal(expected, actual);
    }
    [Fact]
    public void ProductOperatorTest()
    {
        var operatorToken = new OperatorToken("*")
        {
            LeftHand = new NumberToken("123"),
            RightHand = new NumberToken("2")
        };
        var expected = 246;

        var actual = operatorToken.GetValue();

        Assert.Equal(expected, actual);
    }
    [Fact]
    public void DevideOperatorTest()
    {
        var operatorToken = new OperatorToken("/")
        {
            LeftHand = new NumberToken("123"),
            RightHand = new NumberToken("3")
        };
        var expected = 41;

        var actual = operatorToken.GetValue();

        Assert.Equal(expected, actual);
    }
    #endregion
    #region 三項間演算
    [Fact]
    public void TernaryOperations1()
    {
        var operatorToken = new OperatorToken("+")
        {
            LeftHand = new OperatorToken("*")
            {
                LeftHand = new NumberToken("5"),
                RightHand = new NumberToken("2")
            },
            RightHand = new NumberToken("3")
        };
        var expected = 13;

        var actual = operatorToken.GetValue();

        Assert.Equal(expected, actual);
    }
    [Fact]
    public void TernaryOperations2()
    {
        var operatorToken = new OperatorToken("+")
        {
            LeftHand = new NumberToken("3"),
            RightHand = new OperatorToken("/")
            {
                LeftHand = new NumberToken("4"),
                RightHand = new NumberToken("2")
            }
        };
        var expected = 5;

        var actual = operatorToken.GetValue();

        Assert.Equal(expected, actual);
    }
    #endregion
    #region 符号付
    [Fact]
    public void MinusSignedNumber()
    {
        var operatorToken = new OperatorToken("-")
        {
            RightHand = new NumberToken("4")
        };
        var expected = -4;

        var actual = operatorToken.GetValue();

        Assert.Equal(expected, actual);
    }
    [Fact]
    public void MinusSignedNumberOperation()
    {
        var operatorToken = new OperatorToken("+")
        {
            LeftHand = new NumberToken("3"),
            RightHand = new OperatorToken("-")
            {
                RightHand = new NumberToken("2")
            }
        };
        var expected = 1;

        var actual = operatorToken.GetValue();

        Assert.Equal(expected, actual);
    }
    #endregion
    #region 異常系
    [Fact]
    public void BothHandsAreNull()
    {
        var operatorToken = new OperatorToken("+")
        {
            LeftHand = null,
            RightHand = null
        };

        Assert.Throws<UnexpectedTokenException>(() =>
        {
            _ = operatorToken.GetValue();
        });
    }
    [Fact]
    public void RightHandIsNull()
    {
        var operatorToken = new OperatorToken("+")
        {
            LeftHand = new NumberToken("123"),
            RightHand = null
        };

        Assert.Throws<UnexpectedTokenException>(() =>
        {
            _ = operatorToken.GetValue();
        });
    }
    [Fact]
    public void LeftHandIsNullAndOperatorIsProduct()
    {
        var operatorToken = new OperatorToken("*")
        {
            LeftHand = null,
            RightHand = new NumberToken("123")
        };

        Assert.Throws<UnexpectedTokenException>(() =>
        {
            _ = operatorToken.GetValue();
        });
    }
    #endregion
    #endregion
}