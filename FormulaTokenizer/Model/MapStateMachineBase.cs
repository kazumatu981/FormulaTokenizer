namespace FormulaTokenizer.Model;

public abstract class MapStateMachineBase<TState, TOutElement, TInElement>
    : IStateMachine<TState, TOutElement, TInElement>
    where TState : Enum
{
    private readonly TState InitialState;
    protected MapStateMachineBase(TState inilialState)
    {
        InitialState = inilialState;
        State = InitialState;
    }
    public TState State { get; private set; }
    abstract protected TOutElement? ElementMap(TInElement element);
    abstract protected TState GetNextState(TInElement element);
    public virtual void Initialize()
    {
        State = InitialState;
    }

    public virtual TOutElement? GoToNextState(TInElement element)
    {
        var outElement = ElementMap(element);
        State = GetNextState(element);
        return outElement;
    }

    public IEnumerable<TOutElement> Map(IEnumerable<TInElement> elements)
    {
        Initialize();
        foreach (TInElement element in elements)
        {
            if (GoToNextState(element) is TOutElement result)
            {
                yield return result;
            }
        }
    }
}