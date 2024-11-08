using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicTacToeManager : MonoBehaviour
{
    public enum END_GAME_CONDITION{Cross, Donut, Tie}
    [SerializeField, ShowOnly] private bool isCross = true;
    [SerializeField] private Transform boardTrans;
[Header("Player")]
    [SerializeField] private TTT_Pawn crossPlayer;
    [SerializeField] private TTT_Pawn donutPlayer;
[Header("Prefab")]
    [SerializeField] private GameObject crossPrefab;
    [SerializeField] private GameObject donutPrefab;

//棋盘是左下角为0，0，对应info的0
    private char[] layoutInfo = new char[9]{'-','-','-',
                                            '-','-','-',
                                            '-','-','-'};

    private const char CROSS_CHAR = 'X';
    private const char DONUT_CHAR = 'O';
    private const char EMPTY_CHAR = '-';

    private TTT_Pawn currentPlayer;

    void Awake(){
        currentPlayer = isCross?crossPlayer:donutPlayer;
        EventHandler.E_OnSelectGrid += SelectGridHandler;
    }
    void OnDestroy(){
        EventHandler.E_OnSelectGrid -= SelectGridHandler;
    }
    void SelectGridHandler(Vector2Int gridPoint){
        currentPlayer.FinishPlay();

        int index = Geometry.IndexFromGrid(gridPoint);
    //若该位置没有任何棋子，则放置棋子，并检查游戏是否结束
        if(layoutInfo[index] == EMPTY_CHAR){
        //放置棋子
            var piece = Instantiate(isCross?crossPrefab:donutPrefab, boardTrans);
            piece.transform.localPosition = Geometry.PointFromGrid(gridPoint) + Vector3.up*0.4f;
            piece.transform.localRotation = Quaternion.identity;

            layoutInfo[Geometry.IndexFromGrid(gridPoint)] = isCross?CROSS_CHAR:DONUT_CHAR;
        
        //检查游戏是否结束
            char CheckChar = isCross?CROSS_CHAR:DONUT_CHAR;
            if(WinCheck(CheckChar)){
                Debug.Log($"{CheckChar}已经获胜!");
                EventHandler.Call_OnTTTGameEnd(isCross?END_GAME_CONDITION.Cross:END_GAME_CONDITION.Donut);
                return;
            }
            if(StopCheck()){
                Debug.Log("棋盘已满!");
                EventHandler.Call_OnTTTGameEnd(END_GAME_CONDITION.Tie);
                return;
            }

            isCross = !isCross;
            currentPlayer = isCross?crossPlayer:donutPlayer;
            currentPlayer.BeginPlay();
        }
    }
//检查棋盘是否已满
    bool StopCheck(){
        foreach(var piece in layoutInfo){
            if(piece == EMPTY_CHAR){
                return false;
            }
        }
        return true;
    }
//检查是否胜利
    bool WinCheck(char checkSide){
    //row winning check
        for(int i=0; i<3; i++){
            Debug.Log("row");
            int head = i*3;
            if(layoutInfo[head]==checkSide && layoutInfo[head+1]==checkSide && layoutInfo[head+2]==checkSide) return true;
        }
    //col winning check
        for(int i=0; i<3; i++){
            Debug.Log("col");
            if(layoutInfo[i]==checkSide && layoutInfo[i+3]==checkSide && layoutInfo[i+6]==checkSide) return true;
        }
    //Dia winning check
        if(layoutInfo[0]==checkSide && layoutInfo[4]==checkSide && layoutInfo[8]==checkSide) return true;
        if(layoutInfo[2]==checkSide && layoutInfo[4]==checkSide && layoutInfo[6]==checkSide) return true;
    
        return false;
    }
}