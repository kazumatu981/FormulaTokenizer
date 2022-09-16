// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.Serialization;

namespace FormulaTokenizer.Exceptions;

[Serializable]
public class UnexpectedCharException : Exception
{
    public UnexpectedCharException()
           : base()
    {
    }

    public UnexpectedCharException(string message)
        : base(message)
    {
    }

    public UnexpectedCharException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    protected UnexpectedCharException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}