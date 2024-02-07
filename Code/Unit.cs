using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Unit : MonoBehaviour
{
    protected MoveAction moveAction;

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    protected int ActionPoints = 2;
    protected int maxActionPoints = 2;
    protected int TurnsTillGetToMiddle = 2;
    [SerializeField] protected int strength = 3;
    [SerializeField] protected bool isEnemy;
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
    }

    private void Start()
    {
        if (isEnemy) strength -= 1;
            
        // subscribes to the event
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);

        if (isEnemy)
        {
            GetCurrentZone().ChangeControlToEnemy();
        }
    }
    public int GetStrength()
    {
        return strength; 
    }
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public int GetActionPoints()
    {
        return ActionPoints;
    }

    public int GetMaxActionPoints()
    {
        return maxActionPoints;
    }
    public void DoAction(Zone IsWalledZone)
    {
        // checks if zone in which units arrives is wall and if it is it increases points twice
        ActionPoints--;
        if (IsWalledZone.IsWallCheck()) ActionPoints -= 3;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // checks if turn has changed and if it has then it resets action points for all units
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            ActionPoints = maxActionPoints;

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

}
