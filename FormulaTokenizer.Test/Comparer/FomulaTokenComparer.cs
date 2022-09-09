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