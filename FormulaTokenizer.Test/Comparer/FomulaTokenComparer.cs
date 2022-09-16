// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace FormulaTokenizer.Test.Comparer;

public class FormulaTokenComparer : IEqualityComparer<Token>
{
    public bool Equals(Token? x, Token? y)
        => (x, y) switch
        {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            _ => x.Type == y.Type && x.Text == y.Text
        };

    public int GetHashCode(Token _) => throw new NotSupportedException();
}