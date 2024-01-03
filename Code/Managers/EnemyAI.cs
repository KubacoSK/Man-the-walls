using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
   
    private List<Unit> enemyUnits;
    private bool MadeEnemyTurn = false;
    private float EndTurnTimer = 0f;
    private float BetweenMovesTimer = 1f;
    private int currentEnemyIndex = 0;
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
                currentEnemyIndex = 0; // Reset index for the next turn

            }
        }
    }
   

    private bool AllEnemyUnitsHaveCompletedMoves()
    {
        // checks if enemy units have any movement points left and if not, it ends turn
        foreach (Unit enemyUnit in enemyUnits)
        {
            if (enemyUnit != null && enemyUnit.GetActionPoints() < 2)
            {
                return false;
            }
        }
        MadeEnemyTurn = true;
        return true;
        
    }
    public void HandleUnitDestroyed(Unit destroyedUnit)
    {
        // Remove the destroyed unit from the enemyUnits list
        enemyUnits.Remove(destroyedUnit);
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        if (unit != null && unit.IsEnemy())
        {
            enemyUnits.Add(unit);
            Debug.Log("added unit");
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
