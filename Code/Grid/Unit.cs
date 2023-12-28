using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private MoveAction moveAction;
    private int ActionPoints;

    public static event EventHandler OnAnyActionPointsChanged;

    [SerializeField] private bool isEnemy;
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
    }

    private void Start()
    {
        // subscribes to the event
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public int GetActionPoints()
    {
        return ActionPoints;
    }
    public void DoAction(Zone IsWalledZone)
    {
        ActionPoints++;
        if (IsWalledZone.IsWallCheck()) ActionPoints++;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            ActionPoints = 0;

            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }
}
