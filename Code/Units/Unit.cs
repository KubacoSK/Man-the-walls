using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // basic parent class for units, all other units are child of this one
    // also is basic infantry with ability to climb walls and automatic deployment
    protected MoveAction moveAction;
    public enum UnitType { Infantry, Tank, Horseman, BattleRobot, }   // we make units into different classes
    public UnitType TypeOfUnit = UnitType.Infantry;
    // events that handle unit creation and deletion 
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer unitSpriteRenderer;


    protected int MovementCost = 0;
    protected bool canComeToWalls = true;        // if unit is able to climb and defend walls
    protected int ActionPoints = 2;              // movement range of unit
    protected int maxActionPoints = 2;           // how much action points are resetted each turn
    protected int TurnsTillGetToMiddle = 2;      // how often enemy unit chooses to go to the center of the map
    [SerializeField] protected int strength = 3; // how likely unit is to win combat
    [SerializeField] protected bool isEnemy;
    public Zone CurrentStandingZone;
    public int CurrentStandingZoneIndex;
    public static bool hasIncreasedStrength;           // if we applied infantry upgrade
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
    }

    protected void Start()
    {
        if (isEnemy)
        { // checks for difficulty settings and sets stats depending on them
            switch (DifficultySetter.GetDifficulty())
            {
                case "Easy":
                    strength--;
                    break;
                case "Nightmare":
                    strength += 1;
                    break;
            }
        }
        // subscribes to the event
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
        if (hasIncreasedStrength && TypeOfUnit == UnitType.Infantry && !isEnemy) strength++;
        if (isEnemy)
        {
            GetCurrentZone().ChangeControlToEnemy();
        }
    }

    public void SetStandingZone(Zone standingZone, int index)
    {
        CurrentStandingZone = standingZone;
        CurrentStandingZoneIndex = index;
    }
        
    public void SetPastZoneBack()
    {
        if (CurrentStandingZone != null)
            CurrentStandingZone.SetAllyPositionStatus(CurrentStandingZoneIndex, false);
    }
    public void SetEnemyPastZoneBack()
    {
        if (CurrentStandingZone != null)
            CurrentStandingZone.SetEnemyPositionStatus(CurrentStandingZoneIndex, false);
    }
    public Zone ReturnCurrentStandingZone()
    {
        return CurrentStandingZone;
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

    public int GetMovementCost()
    {
        return MovementCost;
    }    

    public int GetMaxActionPoints()
    {
        return maxActionPoints;
    }

    public bool CanComeToWalls()
    {
        return canComeToWalls;
    }
    public void DoAction(Zone IsWalledZone)
    {
        // checks if zone in which units arrives is wall and if it is it increases points twice
        ActionPoints--;
        if (IsWalledZone.IsWallCheck()) ActionPoints -= 3;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    protected void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // checks if turn has changed and if it has then it resets action points for all units
        if ((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) ||
            (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            ActionPoints = maxActionPoints;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public void SetRunningAnimation(bool isRunning)
    {
        if (animator != null)
        {
            animator.SetBool("Running", isRunning);
        }
    }
    public void SetShootingAnimation(bool isShooting)
    {
        if (animator != null)
        {
            animator.SetBool("Shooting", isShooting);
        }
    }
    public bool IsEnemy()
    {
        return isEnemy;

    }
    public void IsDead()
    {
        // triggers an event that removes units and kills it

        animator.SetTrigger("Die"); // Start the death animation

        Destroy(gameObject, 1.4f); // Destroy after animation finishes
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
    public void IncreaseStrength()
    {
         strength++;
    }
    public void FlipUnit()
    {
        unitSpriteRenderer.flipX = true;
    }
    public void FlipUnitBack()
    {
        unitSpriteRenderer.flipX = false;
    }
}
