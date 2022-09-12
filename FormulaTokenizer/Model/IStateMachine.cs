namespace FormulaTokenizer.Model;

public interface IStateMachine<TState, TOutElement, TInElement>
    where TState : Enum
{
    TState State { get; }
    void Initialize();
    TOutElement? GoToNextState(TInElement element);
}