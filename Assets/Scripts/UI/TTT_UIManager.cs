using System.Collections;
using System.Collections.Generic;
using SimpleAudioSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TTT_UIManager : MonoBehaviour
{
[Header("Main Menu")]
    [SerializeField] private CanvasGroup buttonGroup;
    [SerializeField] private Button cancelButton;
[Header("End Screen")]
    [SerializeField] private CanvasGroup endGroup;
    [SerializeField] private TextMeshProUGUI endText;
[Header("Audio")]
    [SerializeField] private AudioSource ui_audio;
    [SerializeField] private string ui_clip;
    void Awake(){
        EventHandler.E_OnTTTGameEnd += TTT_GameEndHandler;
        EventHandler.E_OnAITurn += AITurnHandler;
        EventHandler.E_OnStepChange += StepChangeHandler;
    }
    void OnDestroy(){
        EventHandler.E_OnTTTGameEnd -= TTT_GameEndHandler;
        EventHandler.E_OnAITurn -= AITurnHandler;
        EventHandler.E_OnStepChange -= StepChangeHandler;
    }

#region Button Event
    public void BEvent_Restart(){
        AudioManager.Instance.PlaySoundEffect(ui_audio, ui_clip, 1);
        GameManager.Instance.RestartLevel();
    }
    public void BEvent_BackToMenu(){
        AudioManager.Instance.PlaySoundEffect(ui_audio, ui_clip, 1);
        GameManager.Instance.SwitchingScene("Menu");
    }
    public void BEvent_Cancel(){
        AudioManager.Instance.PlaySoundEffect(ui_audio, ui_clip, 1);
        EventHandler.Call_OnCancelStep();
    }
#endregion

#region EventHandler
    void TTT_GameEndHandler(TicTacToeManager.END_GAME_CONDITION endgame_condition){
        buttonGroup.interactable = false;
        buttonGroup.alpha = 0;
        StartCoroutine(coroutineFadeCanvasGroup(endGroup, 1, 0.5f, true));
        switch(endgame_condition){
            case TicTacToeManager.END_GAME_CONDITION.Cross:
                endText.text = $"{TicTacToeManager.CROSS_CHAR}获胜";
                break;
            case TicTacToeManager.END_GAME_CONDITION.Nought:
                endText.text = $"{TicTacToeManager.NOUGHT_CHAR}获胜";
                break;
            case TicTacToeManager.END_GAME_CONDITION.Tie:
                endText.text = $"平局";
                break;
        }
    }
    void AITurnHandler(bool isTurnBegin){
        cancelButton.interactable = !isTurnBegin;
    }
    void StepChangeHandler(int stepCount){
        if(stepCount <= 1 && cancelButton.interactable)
            cancelButton.interactable = false;
        if(stepCount > 1 && !cancelButton.interactable)
            cancelButton.interactable = true;
    }
#endregion

    IEnumerator coroutineFadeCanvasGroup(CanvasGroup targetGroup, float targetAlpha, float duration, bool turnOnInteractive){
        yield return new WaitForSeconds(0.5f);
        float initAlpha = targetGroup.alpha;
        yield return new WaitForLoop(duration, (t)=>{
            targetGroup.alpha = Mathf.Lerp(initAlpha, targetAlpha, EasingFunc.Easing.SmoothInOut(t));
        });
        targetGroup.interactable = turnOnInteractive;
    }
}
