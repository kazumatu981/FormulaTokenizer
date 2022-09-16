// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

namespace FormulaTokenizer.Exceptions;

[Serializable]
public class UnexpectedTokenException : Exception
{
    public UnexpectedTokenException()
           : base()
    {
    }

    public UnexpectedTokenException(string message)
        : base(message)
    {
    }

    public UnexpectedTokenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected UnexpectedTokenException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}