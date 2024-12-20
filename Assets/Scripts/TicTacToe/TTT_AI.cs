using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class TTT_AI : TTT_Pawn
{
    public async override void BeginPlay(TicTacToeManager ttt_manager)
    {
        EventHandler.Call_OnAITurn(true);
        await Task.Delay(800);
    //计算接下来要走哪一步
        char[] tempLayout = new char[9];
        System.Array.Copy(ttt_manager.m_layoutInfo, tempLayout, 9);
        
        var myChar = ttt_manager.m_isCross?TicTacToeManager.CROSS_CHAR:TicTacToeManager.NOUGHT_CHAR;
        var opponentChar = ttt_manager.m_isCross?TicTacToeManager.NOUGHT_CHAR:TicTacToeManager.CROSS_CHAR;

        int[] availableSlot = ttt_manager.GetAvailableSlotIndex();
    //优先计算下一步自己是否可以胜利
        int index = 0;
        for(int i=0; i<availableSlot.Length; i++){
            index = availableSlot[i];
            tempLayout[index] = myChar;
            if(TicTacToeManager.WinCheck(myChar, tempLayout)){
                EventHandler.Call_OnSelectGrid(Geometry.GridFromIndex(index));
                return;
            }
            tempLayout[index] = TicTacToeManager.EMPTY_CHAR;
        }
    //计算下一步另一方是否可以胜利
        for(int i=0; i<availableSlot.Length; i++){
            index = availableSlot[i];
            tempLayout[index] = opponentChar;
            if(TicTacToeManager.WinCheck(opponentChar, tempLayout)){
                EventHandler.Call_OnSelectGrid(Geometry.GridFromIndex(index));
                return;
            }
            tempLayout[index] = TicTacToeManager.EMPTY_CHAR;
        }

        EventHandler.Call_OnSelectGrid(Geometry.GridFromIndex(availableSlot[Random.Range(0, availableSlot.Length)]));
    }
    public override void FinishPlay(TicTacToeManager ttt_manager)
    {
        base.FinishPlay(ttt_manager);
        EventHandler.Call_OnAITurn(false);
    }
}
