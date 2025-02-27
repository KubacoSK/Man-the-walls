using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Unit : MonoBehaviour
{
    // základná rodičovská trieda pre jednotky, všetky ostatné jednotky sú jej potomkami
    // zároveň predstavuje základnú pechotu s možnosťou liezť na múry a automatickým nasadením
    protected MoveAction moveAction;

    public enum UnitType { Infantry, Tank, Horseman, BattleRobot, }   // rozdelenie jednotiek do rôznych tried
    public UnitType TypeOfUnit = UnitType.Infantry;

    // udalosti, ktoré spracovávajú vytvorenie a zničenie jednotky
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer unitSpriteRenderer;

    protected int MovementCost = 0;
    protected bool canComeToWalls = true;        // či jednotka môže liezť na múry a brániť ich
    protected int ActionPoints = 2;              // rozsah pohybu jednotky
    protected int maxActionPoints = 2;           // počet akčných bodov, ktoré sa obnovujú každý ťah
    protected int TurnsTillGetToMiddle = 2;      // ako často sa nepriateľská jednotka rozhodne ísť do stredu mapy
    [SerializeField] protected int strength = 3; // pravdepodobnosť, že jednotka vyhrá boj
    [SerializeField] protected bool isEnemy;     // označenie, či ide o nepriateľskú jednotku

    public Zone CurrentStandingZone;
    public int CurrentStandingZoneIndex;
    public static bool hasIncreasedStrength;           // ci sme jednotku vylepsili
    private void Awake()
    {
        moveAction = GetComponent<MoveAction>();
    }

    protected void Start()
    {
        if (isEnemy)
        { // zisti obriaznost a podla toho nastavi silu nepriatelskym jednotkam
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
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
        if (hasIncreasedStrength && TypeOfUnit == UnitType.Infantry && !isEnemy) strength++;
        if (isEnemy)
        {
            CurrentStandingZone = GetCurrentZone();
            CurrentStandingZone.ChangeControlToEnemy();
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
        // skontroluje, ci ideme na stenu, ak ano tak nam to zobere movement pointy
        ActionPoints--;
        if (IsWalledZone.IsWallCheck() && isEnemy) ActionPoints -= 3;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    protected void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        // zistime ze ci sa kolo zmenilo, ak ano tak priradime jenotkam plny pocet pohybovych bodov
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
        // event ktory zabije jednotku a odstrani ju

        animator.SetTrigger("Die"); // zaciatok animacie umierania

        Destroy(gameObject, 1.4f);
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
