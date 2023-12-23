using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
   
    private List<Unit> enemyUnits;


    private void Start()
    {
        
        enemyUnits = GetEnemyUnits();
    }
    void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        foreach (Unit Enemyunit in enemyUnits)
        {
            if (Enemyunit != null) MakeDecisionForUnit(Enemyunit);
        }
        if (AllEnemyUnitsHaveCompletedMoves())
        {
            TurnSystem.Instance.NextTurn();
        }
    }
    private void MakeDecisionForUnit(Unit enemyUnit)
    {
        // Get valid zones for the current enemy unit
        List<Zone> validZones = enemyUnit.GetMoveAction().GetValidZonesList();

        // Check if there are valid zones to move to
        if (validZones.Count > 0)
        {
            
            // Randomly choose a destination zone
            Zone destinationZone = validZones[UnityEngine.Random.Range(0, validZones.Count)];
            Vector2 destinationposition = destinationZone.transform.position;
            List<Unit> UnitsInZone = destinationZone.GetUnitsInZone();
            float x = 0;
            foreach (Unit unitinzone in UnitsInZone)
            {
                x += 0.2f;
            }
            destinationposition.x += x;
            // Move the unit towards the chosen zone
            enemyUnit.GetMoveAction().Move(destinationposition);


            StartCoroutine(DelayedSecondMove(enemyUnit, destinationZone));
        }
    }

    private IEnumerator DelayedSecondMove(Unit enemyUnit, Zone destinationZone)
    {
        // Wait for 2 seconds before the second move
        yield return new WaitForSeconds(1.8f);

        // Check if the unit is still valid and has not already made two moves
        if (enemyUnit != null && enemyUnit.GetTurn() < 2)
        {
            // Randomly choose another destination zone for the second move
            List<Zone> validZones = enemyUnit.GetMoveAction().GetValidZonesList();
            if (validZones.Count > 0)
            {
                Zone secondDestinationZone = validZones[UnityEngine.Random.Range(0, validZones.Count)];

                // Move the unit towards the second chosen zone
                enemyUnit.GetMoveAction().Move(secondDestinationZone.transform.position);

                // Increment the turn count for the unit
                enemyUnit.DoTurn();
            }
        }
    }
    private List<Unit> GetEnemyUnits()
    {
        return FindObjectsOfType<Unit>().Where(unit => unit != null && unit.IsEnemy()).ToList();
    }

    private bool AllEnemyUnitsHaveCompletedMoves()
    {
        foreach (Unit enemyUnit in enemyUnits)
        {
            if (enemyUnit != null && enemyUnit.GetTurn() < 2)
            {
                return false;
            }
        }
        return true;
    }
}
