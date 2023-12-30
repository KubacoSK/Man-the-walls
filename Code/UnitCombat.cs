using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class UnitCombat : MonoBehaviour
{
    public static UnitCombat Instance { get; private set; }
    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("There is more than one UnitCombat instance! Destroying the extra one.");
            Destroy(gameObject);
        }

    }
    public void TryEliminateUnits(List<Unit> unitsInZone, Zone thiszone)
    {
        // Check if there are at least two units in the zone
        if (unitsInZone.Count >= 2)
        {
            // Filter units by type (e.g., ally and enemy)
            List<Unit> allyUnits = new List<Unit>();
            List<Unit> enemyUnits = new List<Unit>();

            foreach (Unit unit in unitsInZone)
            {
                if (unit.tag == "Unit")  // You should have a method to determine if the unit is an ally or enemy
                {
                    allyUnits.Add(unit);
                }
                else
                {
                    enemyUnits.Add(unit);
                }
            }

            // If there is at least one ally and one enemy, randomly eliminate one of them
            if (allyUnits.Count > 0 && enemyUnits.Count > 0)
            {
                int allyStrength = allyUnits.Count * 3;  // Change the multiplier based on your balancing
                
                int enemyStrength = enemyUnits.Count * 2;
                if (thiszone.IsWallCheck() == true) allyStrength += 3;

                int randomElementally = Random.Range(0, 7);
                Debug.Log(randomElementally);
                int randomElementenemy = Random.Range(0, 7);
                

                allyStrength += randomElementally;
                Debug.Log("ally strength is " + allyStrength);
                enemyStrength += randomElementenemy;
                Debug.Log("enemy strength is " + enemyStrength);
                // Perform elimination logic (e.g., destroy the unit)
                if (allyStrength > enemyStrength)
                {
                    // Ally wins, eliminate enemy unit
                    EliminateUnit(enemyUnits[0]);
                    EnemyAI.Instance.HandleUnitDestroyed(enemyUnits[0]);
                }
                else if (enemyStrength > allyStrength)
                {
                    // Enemy wins, eliminate ally unit
                    EliminateUnit(allyUnits[0]);
                    EnemyAI.Instance.HandleUnitDestroyed(allyUnits[0]);
                }
                else
                {
                    // Strengths are equal, both units are eliminated
                    EliminateUnit(allyUnits[0]);
                    EliminateUnit(enemyUnits[0]);
                    EnemyAI.Instance.HandleUnitDestroyed(enemyUnits[0]);
                    EnemyAI.Instance.HandleUnitDestroyed(allyUnits[0]);
                }
            }
        }
    }

    private void EliminateUnit(Unit unit)
    {
        // Additional logic for eliminating the unit
        if (unit != null) unit.IsDead();
      
    }
}
