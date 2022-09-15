namespace FormulaTokenizer.Model;

/// <summary>
/// 状態遷移エンジン(ステートマシンエンジン)のインタフェース
/// 現在のステート(状態)を管理し次の状態に遷移する
/// </summary>
/// <typeparam name="TState">状態</typeparam>
/// <typeparam name="TOutElement">遷移時の出力オブジェクト型</typeparam>
/// <typeparam name="TInElement">入力オブジェクト型</typeparam>
public interface IStateMachine<TState, TOutElement, TInElement>
    where TState : Enum
{
    /// <summary>
    /// 状態
    /// </summary>
    /// <value></value>
    TState State { get; }
    /// <summary>
    /// 状態の初期化
    /// </summary>
    void Initialize();
    /// <summary>
    /// 状態を遷移させ、出力を得る
    /// </summary>
    /// <param name="element">入力オブジェクト</param>
    /// <returns>出力オブジェクト</returns>
    TOutElement? GoToNextState(TInElement element);
    /// <summary>
    /// 終了処理
    /// </summary>
    void Uninitialize();
}