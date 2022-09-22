// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace FormulaTokenizer.Test.Comparer;

/// <summary>
/// 
/// </summary>
/// <example>
/// Assert.Equal()で使う場合のサンプル
/// <code>
/// var expected = 1;
/// var actual = (1.0 / 3) * 3.0;
/// 
/// // 小数点第二位まで確認する
/// var comp = new DoubleNearByComparer(0.01);
/// Assert.Equal(expected, actual, comp);
/// </code>
/// </example>
public class DoubleNearByComparer : IEqualityComparer<double>
{
    public readonly double Epsilon;

    public DoubleNearByComparer(double epsilon) => Epsilon = epsilon;

    public bool Equals(double x, double y)
    => Math.Abs(x - y) < Epsilon;

    public int GetHashCode(double _) => throw new NotSupportedException();

}
