using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private MoveAction moveAction;
    private int ActionPoints;
    private int TurnsTillGetToMiddle = 1;

    [SerializeField] private bool IsHorse;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;


    [SerializeField] private bool isEnemy;
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
    }

    private void Start()
    {
        // subscribes to the event
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

        if (isEnemy)
        {
            GetCurrentZone().ChangeControlToEnemy();
        }
    }
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public int GetActionPoints()
    {
        return ActionPoints;
    }
    public void DoAction()
    {
        // does action and increases actionpoints spent
        ActionPoints++;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
    public void DoAction(Zone IsWalledZone)
    {
        // checks if zone in which units arrives is wall and if it is it increases points twice
        ActionPoints++;
        if (IsWalledZone.IsWallCheck()) ActionPoints += 3;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // checks if turn has changed and if it has then it resets action points for all units
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
    public void IsDead()
    {
        // triggers an event that removes units and kills it
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }
    public Zone GetCurrentZone()
    {
        Collider2D collider = Physics2D.OverlapPoint(transform.position, LayerMask.GetMask("GridPoints"));

        if (collider != null)
        {
            return collider.GetComponent<Zone>();
        }

        return null;
    }

    public int GetTurnMiddlePoints()
    {
        return TurnsTillGetToMiddle;
    }

    public void SetTurnMiddlePoints(int increaseNumber)
    {
        TurnsTillGetToMiddle += increaseNumber;
    }

    public bool GetHorse()
    {
        return IsHorse;
    }
}
