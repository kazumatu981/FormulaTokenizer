namespace FormulaTokenizer.Model;

public abstract class MapReduceStateMachineBase<TState, TResult, TOutElement, TInElement>
    : MapStateMachineBase<TState, TOutElement, TInElement>
    where TState : Enum
{
    protected MapReduceStateMachineBase(TState inilialState) : base(inilialState)
    {
        // Nothing Todo
    }

    public TResult? MapReduce(IEnumerable<TInElement> elements, TResult? seed)
    {
        var mapResults = Map(elements);
        TResult? result = seed;
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