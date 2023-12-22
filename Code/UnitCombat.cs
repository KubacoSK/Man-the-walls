using System.Collections;
using System.Collections.Generic;
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
    public void TryEliminateUnits(List<Unit> unitsInZone)
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
                Unit unitToEliminate = Random.Range(0, 2) == 0 ? allyUnits[0] : enemyUnits[0];

                // Perform elimination logic (e.g., destroy the unit)
                EliminateUnit(unitToEliminate);
            }
        }
    }

    private void EliminateUnit(Unit unit)
    {
        // Additional logic for eliminating the unit
        if(unit!= null)  Destroy(unit.gameObject);
      
    }
}
