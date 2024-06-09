using System.Collections;
using System.Collections.Generic;
using System.Security;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class UnitCombat : MonoBehaviour
{
    public static UnitCombat Instance { get; private set; }
    public void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one PauseMenu! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }
    public void TryEliminateUnits(List<Unit> unitsInZone, Zone thiszone)
    {
       
        
            Debug.Log("executing");
        // Check if there are at least two units in the zone
        // Filter units by type (e.g., ally and enemy)
        List<Unit> allyUnits = new List<Unit>();
        List<Unit> enemyUnits = new List<Unit>();
        // fills the lists with numbers of units inside the zone
        foreach (Unit unit in unitsInZone)
        {
            if (unit.tag == "Unit")
            {
                allyUnits.Add(unit);
            }
            else
            {
                enemyUnits.Add(unit);
            }
        }
        float totalunits = (allyUnits.Count + enemyUnits.Count) / 3;
        bool? alliesWon = null;  // adds counter that if enemies won last fight allies get buff and reversed
                                 // If there is at least one ally and one enemy, randomly eliminate one of them
            for (int i = 0; i <= totalunits; i++)
            {
                if (allyUnits.Count > 0 && enemyUnits.Count > 0)
                {
                    int allyStrength = 0;
                    int enemyStrength = 0;
                    if (alliesWon == true) allyStrength -= 2;
                    if (alliesWon == false) enemyStrength -= 2;
                    foreach (Unit unit in allyUnits) allyStrength += unit.GetStrength(); // increases allied strength based number of allies in zone
                    foreach (Unit unit in enemyUnits) enemyStrength += unit.GetStrength();
                    if (thiszone.IsWallCheck() == true) allyStrength += 3; // if we are fighting on a wall we add more power
                    if (thiszone.IsWallCheck() && Zone.isWallUpgraded) allyStrength++;  // adds more combat power if zone is upgraded with better walls

                    int randomElementally = Random.Range(0, 7);
                    int randomElementenemy = Random.Range(0, 7);

                    allyStrength += randomElementally;
                    enemyStrength += randomElementenemy;
                    // Perform elimination logic (e.g., destroy the unit)
                    if (allyStrength > enemyStrength)
                    {
                        // Ally wins, eliminate enemy unit
                        Unit enemyUnit = enemyUnits[0];
                        enemyUnits.Remove(enemyUnit);
                        EliminateUnit(enemyUnit);
                        EnemyAI.Instance.HandleUnitDestroyed(enemyUnit);
                        alliesWon = true;
                    }
                    else if (enemyStrength > allyStrength)
                    {
                        // Enemy wins, eliminate ally unit
                        Unit allyUnit = allyUnits[0];
                        allyUnits.Remove(allyUnit);
                        EliminateUnit(allyUnit);
                        alliesWon = false;
                    }
                    else
                    {
                        // Strengths are equal, both units are eliminated
                        Unit allyUnit = allyUnits[0];
                        Unit enemyUnit = enemyUnits[0];
                        allyUnits.Remove(allyUnit);
                        enemyUnits.Remove(enemyUnit);
                        EliminateUnit(enemyUnit);
                        EliminateUnit(allyUnit);
                        EnemyAI.Instance.HandleUnitDestroyed(enemyUnit);
                        alliesWon = null;
                    }
                    if (allyUnits.Count == 0)
                    {
                        thiszone.ChangeControlToEnemy(); // we change control of the zone if all enemies are wiped out
                    }
                    else if (enemyUnits.Count == 0)
                    {
                        thiszone.ChangeControlToAlly();
                    }
                }
            }
        
    }

    private void EliminateUnit(Unit unit)
    {
        // Additional logic for eliminating the unit
        if (unit != null) unit.IsDead();
        if (unit.IsEnemy()) unit.SetEnemyPastZoneBack();
        if (!unit.IsEnemy()) unit.SetPastZoneBack();
    }
}
