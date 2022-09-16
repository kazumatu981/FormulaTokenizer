// (c) Kazuyoshi Matsumoto.
// Kazuyoshi Matsumoto licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace FormulaTokenizer.Model;

/// <summary>
/// 列挙型オブジェクトを順に処理し出力するステートマシンのベースクラス
/// </summary>
/// <typeparam name="TState">状態型</typeparam>
/// <typeparam name="TOutElement">出力オブジェクト型</typeparam>
/// <typeparam name="TInElement">入力オブジェクト型</typeparam>
public abstract class MapStateMachineBase<TState, TOutElement, TInElement>
    : IStateMachine<TState, TOutElement, TInElement>
    where TState : Enum
{
    /// <summary>
    /// 初期状態
    /// </summary>
    private readonly TState _initialState;
    /// <summary>
    /// 基本コンストラクタ。初期状態を指定してステートマシンを生成する。
    /// </summary>
    /// <param name="inilialState">初期状態</param>
    protected MapStateMachineBase(TState inilialState)
    {
        _initialState = inilialState;
        State = _initialState;
    }
    #region Public Members
    #region Implementation Of IStateMachine
    /// <summary>
    /// 状態
    /// </summary>
    /// <value></value>
    public TState State { get; private set; }
    /// <summary>
    /// 初期化
    /// </summary>
    public virtual void Initialize()
    {
        State = _initialState;
    }
    public abstract void Uninitialize();

    /// <summary>
    /// 状態遷移の実行
    /// </summary>
    /// <param name="element">入力オブジェクト</param>
    /// <returns>出力オブジェクト</returns>
    public virtual TOutElement? GoToNextState(TInElement element)
    {
        var outElement = ElementMap(element);
        State = GetNextState(element);
        return outElement;
    }
    #endregion

    /// <summary>
    /// 入力列挙から出力列挙を得る。ただし、要素のうちnullになる要素はスキップされる
    /// </summary>
    /// <param name="elements">入力オブジェクトの列挙</param>
    /// <returns>出力オブジェクトの列挙</returns>
    public IEnumerable<TOutElement> Map(IEnumerable<TInElement> elements)
    {
        Initialize();
        foreach (var element in elements)
        {
            if (GoToNextState(element) is TOutElement result)
            {
                yield return result;
            }
        }
        Uninitialize();
    }

    #endregion

    #region Protected Member
    /// <summary>
    /// 各要素の状態による出力を算出する。
    /// </summary>
    /// <param name="element">入力オブジェクト</param>
    /// <returns>出力オブジェクト</returns>
    protected abstract TOutElement? ElementMap(TInElement element);
    /// <summary>
    /// 現在の状態から次の状態を算出する。
    /// </summary>
    /// <param name="element">入力オブジェクト</param>
    /// <returns>次の状態</returns>
    protected abstract TState GetNextState(TInElement element);
    #endregion
}