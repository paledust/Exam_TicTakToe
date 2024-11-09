using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UI_Button_HoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI buttonText;
    private Button button;
    private CoroutineExcuter UIFader;
    
    void Awake(){
        button = GetComponent<Button>();
        UIFader = new CoroutineExcuter(this);
    }
    public void OnPointerEnter(PointerEventData eventData){
        if(button.interactable){
            UIFader.Excute(CommonCoroutine.coroutineFadeUI(buttonText, 1, 0.1f));
        }
    }
    public void OnPointerExit(PointerEventData eventData){
        UIFader.Excute(CommonCoroutine.coroutineFadeUI(buttonText, 0, 0.1f));
    }
}
