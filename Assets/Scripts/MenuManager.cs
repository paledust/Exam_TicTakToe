using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster menuRaycaster;
    public void BEvent_Quit(){
        GameManager.Instance.EndGame();
        menuRaycaster.enabled = false;
    }
    public void BEvent_VS_Player(){
        menuRaycaster.enabled = false;
        TicTacToeManager.m_GameMode = TicTacToeManager.GAME_MODE.VS_HUMAN;
        GameManager.Instance.SwitchingScene("Game");
    }
    public void BEvent_VS_NPC(){
        menuRaycaster.enabled = false;
        TicTacToeManager.m_GameMode = TicTacToeManager.GAME_MODE.VS_AI;
        GameManager.Instance.SwitchingScene("Game");
    }
}
