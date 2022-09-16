// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace FormulaTokenizer.Model;

public abstract class MapReduceStateMachineBase<TState, TResult, TOutElement, TInElement>
    : MapStateMachineBase<TState, TOutElement, TInElement>
    where TState : Enum
{
    protected MapReduceStateMachineBase(TState inilialState) : base(inilialState)
    {
        // Nothing to-do
    }

    public TResult? MapReduce(IEnumerable<TInElement> elements, TResult? seed)
    {
        var mapResults = Map(elements);
        var result = seed;
        foreach (var mapResult in mapResults)
        {
            if (ElementReduce(result, mapResult) is TResult reduced)
            {
                result = reduced;
            }
        }
        return result;
    }

    protected abstract TResult? ElementReduce(
        TResult? previousResult, TOutElement nextElement);
}