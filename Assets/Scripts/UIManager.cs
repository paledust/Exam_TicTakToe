using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup buttonGroup;
    void Awake(){
        EventHandler.E_AfterLoadScene += AfterLoadSceneHandler;
        EventHandler.E_BeforeUnloadScene += BeforeLoadSceneHandler;
        
    }
    void OnDestroy(){
        EventHandler.E_AfterLoadScene -= AfterLoadSceneHandler;
        EventHandler.E_BeforeUnloadScene -= BeforeLoadSceneHandler;
    }
    public void BEvent_Restart(){
        GameManager.Instance.RestartLevel();
    }
    public void BEvent_BackToMenu(){
        GameManager.Instance.SwitchingScene("Menu");
    }
    public void BEvent_Cancel(){
    }
    void BeforeLoadSceneHandler(){
        buttonGroup.interactable = false;
    }
    void AfterLoadSceneHandler(){
        if(GameManager.Instance.currentScene == "Menu"){
            buttonGroup.interactable = false;
            buttonGroup.alpha = 0;
        }
        else{
            buttonGroup.interactable = true;
            buttonGroup.alpha = 1;
        }
    }
}
