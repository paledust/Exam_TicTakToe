using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public void BEvent_Quit(){
        GameManager.Instance.EndGame();
    }
    public void BEvent_VS_Player(){
        GameManager.Instance.SwitchingScene("Game_Player");
    }
    public void BEvent_VS_NPC(){
        GameManager.Instance.SwitchingScene("Game_NPC");
    }
}
