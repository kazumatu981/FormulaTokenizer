// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using FormulaTokenizer.Exceptions;

namespace FormulaTokenizer;

public enum OperatorKind
{
    Plus, Minus, Product, Devide
}
public class OperatorToken : Token
{
    public const string Plus = "+";
    public const string Minus = "-";
    public const string Product = "*";
    public const string Devide = "/";

    public readonly OperatorKind Kind;
    public Token? LeftHand { get; set; }
    public Token? RightHand { get; set; }
    public bool IsPlusMinus => Kind == OperatorKind.Plus || Kind == OperatorKind.Minus;

    public OperatorToken(string text) : base(TokenType.Operator, text)
    {
        Kind = text switch
        {
            // 四則演算子
            Plus => OperatorKind.Plus,
            Minus => OperatorKind.Minus,
            Product => OperatorKind.Product,
            Devide => OperatorKind.Devide,

            // それ以外は予期せぬ文字
            _ => throw new UnexpectedCharException()
        };
    }

    public override int GetValue()
        => (LeftHand, RightHand, Kind) switch
        {
            // No Right-Left hands
            (null, null, _) => throw new UnexpectedTokenException(),

            // No Right Hand
            (_, null, _) => throw new UnexpectedTokenException(),

            // No Left Hand, Sign Blanch
            (null, _, OperatorKind.Plus) => RightHand.GetValue(),
            (null, _, OperatorKind.Minus) => (-1) * RightHand.GetValue(),

            // No Left Hand but not Sign Blanch
            (null, _, _) => throw new UnexpectedTokenException(),

            // 通常の四則演算
            (_, _, OperatorKind.Plus) => LeftHand.GetValue() + RightHand.GetValue(),
            (_, _, OperatorKind.Minus) => LeftHand.GetValue() - RightHand.GetValue(),
            (_, _, OperatorKind.Product) => LeftHand.GetValue() * RightHand.GetValue(),
            (_, _, OperatorKind.Devide) => LeftHand.GetValue() / RightHand.GetValue(),

            // 予期せぬ例外(理論上ありえないパス)
            _ => throw new NotImplementedException()
        };
}