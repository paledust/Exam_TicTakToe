using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A basic C# Event System
public static class EventHandler
{
#region Basic Event
    public static event Action E_BeforeUnloadScene;
    public static void Call_BeforeUnloadScene()=>E_BeforeUnloadScene?.Invoke();
    public static event Action E_AfterLoadScene;
    public static void Call_AfterLoadScene()=>E_AfterLoadScene?.Invoke();
    public static event Action E_OnBeginSave;
    public static void Call_OnBeginSave()=>E_OnBeginSave?.Invoke();
    public static event Action E_OnCompleteSave;
    public static void Call_OnCompleteSave()=>E_OnCompleteSave?.Invoke();
#endregion

#region TTT Event
    public static event Action<bool> E_OnAITurn;
    public static void Call_OnAITurn(bool isTurnBegin)=>E_OnAITurn?.Invoke(isTurnBegin);
    public static event Action<int> E_OnStepChange;
    public static void Call_OnStepChange(int stepCount)=>E_OnStepChange?.Invoke(stepCount);
    public static event Action E_OnCancelStep;
    public static void Call_OnCancelStep()=>E_OnCancelStep?.Invoke();
    public static event Action<Vector2Int> E_OnSelectGrid;
    public static void Call_OnSelectGrid(Vector2Int gridPoint)=>E_OnSelectGrid?.Invoke(gridPoint);
    public static event Action<TicTacToeManager.END_GAME_CONDITION> E_OnTTTGameEnd;
    public static void Call_OnTTTGameEnd(TicTacToeManager.END_GAME_CONDITION endCondition)=>E_OnTTTGameEnd?.Invoke(endCondition);
#endregion
}