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

    private void Start()
    {
        Instance = this;
        enemyUnits = UnitManager.Instance.GetEnemyUnitList();
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
                    MakeDecisionForUnit(enemyUnit);
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
    private void MakeDecisionForUnit(Unit enemyUnit)
    {
        // Get valid zones for the current enemy unit
        List<Zone> validZones = enemyUnit.GetMoveAction().GetValidZonesList();

        // Check if there are valid zones to move to
        if (validZones.Count > 0)
        {
            
            // Randomly choose a destination zone
            Zone destinationZone = validZones[UnityEngine.Random.Range(0, validZones.Count)];
            bool StayStill = false;
            foreach (Zone zone in validZones)
            {
                if (zone.ReturnAllyUnitsInZone().Count > 0)
                {
                    destinationZone = zone;
                    StayStill = true;
                }
            }
            Vector2 destinationposition = destinationZone.transform.position;
            List<Unit> UnitsInZone = destinationZone.GetUnitsInZone();
            float x = 0;
            float y = 0;
            foreach (Unit unitinzone in UnitsInZone)
            {
                x += 0.4f;
            }
            if (destinationZone.GetZoneSizeModifier().x == 1) y -= 0.4f;

            destinationposition.x += x;
            destinationposition.y += y;
            // Move the unit towards the chosen zone
            enemyUnit.GetMoveAction().Move(destinationposition);
            enemyUnit.DoAction(destinationZone);
            if(!StayStill)StartCoroutine(DelayedSecondMove(enemyUnit));
            else { enemyUnit.DoAction(); }
            
            
        }
        Camera.main.transform.position = enemyUnit.transform.position + new Vector3(0, 0, -10);
    }

    private IEnumerator DelayedSecondMove(Unit enemyUnit)
    {
        // Wait for 2 seconds before the second move
        yield return new WaitForSeconds(1.5f);

        // Check if the unit is still valid and has not already made two moves
        if (enemyUnit != null && enemyUnit.GetActionPoints() < 2)
        {
            // Randomly choose another destination zone for the second move
            List<Zone> validZones2 = enemyUnit.GetMoveAction().GetValidZonesList();
            if (validZones2.Count > 0)
            {
                Zone seconddestinationZone = validZones2[UnityEngine.Random.Range(0, validZones2.Count)];
                foreach (Zone zone in validZones2)
                {
                    if (zone.ReturnAllyUnitsInZone().Count > 0)
                    {
                        seconddestinationZone = zone;
                    }
                }
                Vector2 destinationposition = seconddestinationZone.transform.position;
                List<Unit> UnitsInZone2 = seconddestinationZone.GetUnitsInZone();
                float sx = 0;
                foreach (Unit unitinzone in UnitsInZone2)
                {
                    sx += 0.4f;
                }
                destinationposition.x += sx;
                // Move the unit towards the chosen zone
                enemyUnit.GetMoveAction().Move(destinationposition);
                enemyUnit.DoAction(seconddestinationZone);
            }
        }
    }

    private bool AllEnemyUnitsHaveCompletedMoves()
    {
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
}
