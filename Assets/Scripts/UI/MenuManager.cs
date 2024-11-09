using System.Collections;
using System.Collections.Generic;
using SimpleAudioSystem;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster menuRaycaster;
    [SerializeField] private AudioSource ui_audio;
    [SerializeField] private string uiClip;
    public void BEvent_Quit(){
        AudioManager.Instance.PlaySoundEffect(ui_audio, uiClip, 1);
        GameManager.Instance.EndGame();
        menuRaycaster.enabled = false;
    }
    public void BEvent_VS_Player(){
        AudioManager.Instance.PlaySoundEffect(ui_audio, uiClip, 1);
        menuRaycaster.enabled = false;
        TicTacToeManager.m_GameMode = TicTacToeManager.GAME_MODE.VS_HUMAN;
        GameManager.Instance.SwitchingScene("Game");
    }
    public void BEvent_VS_NPC(){
        AudioManager.Instance.PlaySoundEffect(ui_audio, uiClip, 1);
        menuRaycaster.enabled = false;
        TicTacToeManager.m_GameMode = TicTacToeManager.GAME_MODE.VS_AI;
        GameManager.Instance.SwitchingScene("Game");
    }
}
