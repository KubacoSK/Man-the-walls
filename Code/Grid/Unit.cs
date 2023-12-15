using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private MoveAction moveAction;
    private int turn;
    private void Awake()
    { 
        moveAction = GetComponent<MoveAction>();
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }
    private bool IsOnZone()
    {
        // Get the current position of the unit
        Vector2 unitPosition = transform.position;

        // Perform overlap check with the "Zone" layer (you can customize the layer name)
        Collider2D hitCollider = Physics2D.OverlapPoint(unitPosition, LayerMask.GetMask("GridPoints"));

        // Check if a collider was hit
        return hitCollider != null;
    }
    public int GetTurn()
    {
        return turn;
    }
    public void DoTurn()
    {
        turn++;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        turn = 0;
    }
}
