using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
   
    private List<Unit> enemyUnits; 
    private bool MadeEnemyTurn = false;                 // zmeni sa ak jednotka spravi svoje kolo
    private float EndTurnTimer = 0f;                    // po tom co sa vsetky jednotky pohnu konci kolo o 2 sekundy
    private float BetweenMovesTimer = 1f;               // cas medzi nepriatelskymi pohybmi
    private int currentEnemyIndex = 0;                  // ktora nepriatelska jednotka robi svoje kolo
    public static EnemyAI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one EnemyAi! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        enemyUnits = new List<Unit>();
    }
    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }
    void Update()
    {

        if (TurnSystem.Instance.IsPlayerTurn())
        {
            MadeEnemyTurn = false;
            return;
        }

        BetweenMovesTimer += Time.deltaTime;

        if (!MadeEnemyTurn && BetweenMovesTimer > 2.5f)
        {
            BetweenMovesTimer = 0f;

            if (currentEnemyIndex < enemyUnits.Count)
            {
                Unit enemyUnit = enemyUnits[currentEnemyIndex];

                if (enemyUnit != null)
                {
                    EnemyAiMove.Instance.MakeDecisionForUnit(enemyUnit);
                    TurnSystemUI.Instance.UpdateEnemySoldiersTurnNumber(enemyUnits.Count - currentEnemyIndex);
                    currentEnemyIndex++;
                }
            }
        }
        if(AllEnemyUnitsHaveCompletedMoves())
        {
            EndTurnTimer += Time.deltaTime;

            if (EndTurnTimer > 2f)
            {
                TurnSystem.Instance.NextTurn();
                EndTurnTimer = 0f;
                currentEnemyIndex = 0; // resetuje index pre dalsie kolo

            }
        }
    }
   

    private bool AllEnemyUnitsHaveCompletedMoves()
    {
        // kontroluje ci jednotky vsetky spravili svoj tah
        foreach (Unit enemyUnit in enemyUnits)
        {
            if (enemyUnit != null && enemyUnit.GetActionPoints() > 0)
            {
                return false;
            }
        }
        MadeEnemyTurn = true;
        return true;
        
    }
    public void HandleUnitDestroyed(Unit destroyedUnit)
    {
        enemyUnits.Remove(destroyedUnit);
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        if (unit != null && unit.IsEnemy())
        {
            enemyUnits.Add(unit);
        }
    }
    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        if (unit != null && unit.IsEnemy())
        {
            enemyUnits.Remove(unit);
        }
    }
}
