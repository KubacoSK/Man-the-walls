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
                int allyStrength = allyUnits.Count * 3;  // Change the multiplier based on your balancing
                
                int enemyStrength = enemyUnits.Count * 2;
               

                int randomElementally = Random.Range(0, 7);
                
                int randomElementenemy = Random.Range(0, 7);
                

                allyStrength += randomElementally;
                Debug.Log("ally strength is " + allyStrength);
                enemyStrength += randomElementenemy;
                Debug.Log("enemy strength is " + enemyStrength);
                // Perform elimination logic (e.g., destroy the unit)
                if (allyStrength > enemyStrength)
                {
                    // Ally wins, eliminate enemy unit
                    EliminateUnit(enemyUnits[Random.Range(0, enemyUnits.Count)]);
                 
                }
                else if (enemyStrength > allyStrength)
                {
                    // Enemy wins, eliminate ally unit
                    EliminateUnit(allyUnits[Random.Range(0, allyUnits.Count)]);
                   
                }
                else
                {
                    // Strengths are equal, both units are eliminated
                    EliminateUnit(allyUnits[Random.Range(0, allyUnits.Count)]);
                    EliminateUnit(enemyUnits[Random.Range(0, enemyUnits.Count)]);
                    
                }
            }
        }
    }

    private void EliminateUnit(Unit unit)
    {
        // Additional logic for eliminating the unit
        if(unit!= null)  Destroy(unit.gameObject);
      
    }
}
