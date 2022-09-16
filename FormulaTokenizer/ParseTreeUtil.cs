// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace FormulaTokenizer;

public static class ParserUtil
{
    public static ParseTree? Parse(this IEnumerable<Token> tokenized)
        => (new Parser()).MapReduce(tokenized, new ParseTree());
}