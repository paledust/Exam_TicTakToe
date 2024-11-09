using System.Collections;
using System.Collections.Generic;
using SimpleAudioSystem;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class TicTacToeManager : MonoBehaviour
{
    public enum END_GAME_CONDITION{Cross, Nought, Tie}
    public enum GAME_MODE{VS_AI, VS_HUMAN}
[Header("Player")]
    [SerializeField] private GameObject ai_prefab;
    [SerializeField] private GameObject player_prefab;

[Header("Prefab")]
    [SerializeField] private GameObject crossPrefab;
    [SerializeField] private GameObject noughtPrefab;

[Header("Visual")]
    [SerializeField] private Transform boardTrans;
    [SerializeField] private SpriteRenderer backgroundSprite;
    [SerializeField] private Color crossColor;
    [SerializeField] private Color noughtColor;

[Header("Audio")]
    [SerializeField] private AudioSource sfx_audio;
    [SerializeField] private string piece_clip;

//棋盘是左下角为0，0，对应info的0
    public char[] m_layoutInfo{get; private set;} = new char[9]{'-','-','-',
                                                                '-','-','-',
                                                                '-','-','-'};
    private GameObject[] pieces = new GameObject[9];                                                    
    private TTT_Pawn currentPlayer;
    private TTT_Pawn crossPlayer;
    private TTT_Pawn noughtPlayer;
    private Stack<Vector2Int> stackMoves;
    private CoroutineExcuter backgroundChanger;

    public const char CROSS_CHAR = 'X';
    public const char NOUGHT_CHAR = 'O';
    public const char EMPTY_CHAR = '-';

    public bool m_isCross{get{return stackMoves==null || (stackMoves.Count%2 == 0);}}

    public Transform m_boardTrans{get{return boardTrans;}}

    public static GAME_MODE m_GameMode = GAME_MODE.VS_AI;

//检查该布局是否存在胜利情况
    public static bool WinCheck(char checkSide, char[] layoutInfo){
    //检查每行是否有胜利
        for(int i=0; i<3; i++){
            int head = i*3;
            if(layoutInfo[head]==checkSide && layoutInfo[head+1]==checkSide && layoutInfo[head+2]==checkSide) return true;
        }
    //检查每列是否有胜利
        for(int i=0; i<3; i++){
            if(layoutInfo[i]==checkSide && layoutInfo[i+3]==checkSide && layoutInfo[i+6]==checkSide) return true;
        }
    //检查对角是否有胜利
        if(layoutInfo[0]==checkSide && layoutInfo[4]==checkSide && layoutInfo[8]==checkSide) return true;
        if(layoutInfo[2]==checkSide && layoutInfo[4]==checkSide && layoutInfo[6]==checkSide) return true;
    
        return false;
    }

    void Awake(){
        EventHandler.E_OnSelectGrid += SelectGridHandler;
        EventHandler.E_OnCancelStep += CancelStepHandler;

        stackMoves = new Stack<Vector2Int>();
        backgroundChanger = new CoroutineExcuter(this);
    //生成玩家
        TTT_Pawn player_one, player_two;
        switch(m_GameMode){
            case GAME_MODE.VS_HUMAN:
                player_one = Instantiate(player_prefab).GetComponent<TTT_Player>();
                player_two = Instantiate(player_prefab).GetComponent<TTT_Player>();

                break;
            default:
                player_one = Instantiate(player_prefab).GetComponent<TTT_Player>();
                player_two = Instantiate(ai_prefab).GetComponent<TTT_AI>();

                break;
        }
    //决定先手
        bool firstIsCross = (Random.value - 0.5f)>0;
        crossPlayer = firstIsCross?player_one:player_two;
        noughtPlayer = firstIsCross?player_two:player_one;

        currentPlayer = crossPlayer;
    }
    void OnDestroy(){
        EventHandler.E_OnSelectGrid -= SelectGridHandler;
        EventHandler.E_OnCancelStep -= CancelStepHandler;
    }
    void Start(){
        EventHandler.Call_OnStepChange(stackMoves.Count);
        currentPlayer.BeginPlay(this);
        backgroundChanger.Excute(CommonCoroutine.coroutineFadeSpriteColor(backgroundSprite, crossColor, 0.25f));
    }
#region Event Handler
    void CancelStepHandler(){
        var move = stackMoves.Pop();
        RemovePiece(move);
        move = stackMoves.Pop();
        RemovePiece(move);
        EventHandler.Call_OnStepChange(stackMoves.Count);
        
        currentPlayer.BeginPlay(this);
    }
    void SelectGridHandler(Vector2Int gridPoint){
        int index = Geometry.IndexFromGrid(gridPoint);
    //若该位置没有任何棋子，则放置棋子，并检查游戏是否结束
        if(m_layoutInfo[index] == EMPTY_CHAR){
            currentPlayer.FinishPlay(this);
        //放置棋子
            var piece = Instantiate(m_isCross?crossPrefab:noughtPrefab, boardTrans);
            piece.transform.localPosition = Geometry.PointFromGrid(gridPoint) + Vector3.up*0.4f;
            piece.transform.localRotation = Quaternion.identity;
            pieces[index] = piece;
            m_layoutInfo[index] = m_isCross?CROSS_CHAR:NOUGHT_CHAR;
        
            bool isCross = m_isCross;
            stackMoves.Push(gridPoint);
            EventHandler.Call_OnStepChange(stackMoves.Count);

            AudioManager.Instance.PlaySoundEffect(sfx_audio, piece_clip, 1);
        //检查游戏是否结束
            char CheckChar = isCross?CROSS_CHAR:NOUGHT_CHAR;
            if(WinCheck(CheckChar, m_layoutInfo)){
                Debug.Log($"{CheckChar}已经获胜!");
                EventHandler.Call_OnTTTGameEnd(isCross?END_GAME_CONDITION.Cross:END_GAME_CONDITION.Nought);
                return;
            }
        //检查棋盘是否满
            if(stackMoves.Count >= 9){
                Debug.Log("棋盘已满!");
                EventHandler.Call_OnTTTGameEnd(END_GAME_CONDITION.Tie);
                return;
            }

            currentPlayer = m_isCross?crossPlayer:noughtPlayer;
            backgroundChanger.Excute(CommonCoroutine.coroutineFadeSpriteColor(backgroundSprite, m_isCross?crossColor:noughtColor, 0.1f));
            currentPlayer.BeginPlay(this);
        }
    }
#endregion

    void RemovePiece(Vector2Int gridPoint){
        int index = Geometry.IndexFromGrid(gridPoint);
        Destroy(pieces[index].gameObject);
        pieces[index] = null;
        m_layoutInfo[index] = EMPTY_CHAR;
    }
    public bool HasPiece(Vector2Int gridPoint){
        return m_layoutInfo[Geometry.IndexFromGrid(gridPoint)] != EMPTY_CHAR;
    }
    public int[] GetAvailableSlotIndex(){
        List<int> pendingSlots = new List<int>();
        for(int i=0; i<m_layoutInfo.Length; i++){
            if(m_layoutInfo[i] == EMPTY_CHAR) pendingSlots.Add(i);
        }
        return pendingSlots.ToArray();
    }
}