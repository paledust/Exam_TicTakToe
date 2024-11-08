using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TTT_Player : TTT_Pawn
{
    [SerializeField] private Transform boardTransform;
    [SerializeField] private GameObject highLighterPrefab;

    private GameObject highLighter;
    private Vector2Int selectedGrid;

    void Start(){
        highLighter = Instantiate(highLighterPrefab, boardTransform);

        highLighter.transform.localPosition =  Geometry.PointFromGrid(new Vector2Int(0,0));
        highLighter.transform.localRotation =  Quaternion.identity;
        highLighter.SetActive(false);
    }
    void Update(){
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, Service.INTERACT_LAYER)){
            Vector2Int gridPoint = Geometry.GridFromPoint(boardTransform.InverseTransformPoint(hit.point));
            if(Geometry.ValidPoint(gridPoint)){
                highLighter.SetActive(true);
                highLighter.transform.localPosition = Geometry.PointFromGrid(gridPoint)+Vector3.up*0.4f;
                
                selectedGrid = gridPoint;
            }
            else{
                highLighter.SetActive(false);
                selectedGrid = new Vector2Int(-1, -1);
            }
        }
        else{
            highLighter.SetActive(false);
            selectedGrid = new Vector2Int(-1, -1);
        }
    }
    public override void BeginPlay(TicTacToeManager ttt_manager){
        this.enabled = true;
    }
    public override void FinishPlay(TicTacToeManager ttt_manager){
        this.enabled = false;
        highLighter.SetActive(false);
    }
#region Input
    void OnSelect(InputValue input){
        if(input.isPressed && Geometry.ValidPoint(selectedGrid)){
            EventHandler.Call_OnSelectGrid(selectedGrid);
        }
    }
#endregion
}