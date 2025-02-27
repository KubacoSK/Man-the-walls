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
            Debug.LogError("Existuje viac ako jedno PauseMenu! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void TryEliminateUnits(List<Unit> unitsInZone, Zone thiszone)
    {
        Debug.Log("Vykonávam");

        // Skontroluj, či sú v zóne aspoň dve jednotky
        // Filtrovanie jednotiek podľa typu (napr. spojenci a nepriatelia)
        List<Unit> allyUnits = new List<Unit>();
        List<Unit> enemyUnits = new List<Unit>();

        // Naplnenie zoznamov počtom jednotiek v zóne
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
        bool? alliesWon = null;  // pridáva čítač, ktorý, ak nepriatelia vyhrali posledný boj, dá spojencom buff a naopak
                                 // Ak je v zóne aspoň jedna jednotka spojenca a jedna nepriateľa, náhodne eliminuj jednu z nich
        for (int i = 0; i <= totalunits; i++)
        {
            if (allyUnits.Count > 0 && enemyUnits.Count > 0)
            {
                int allyStrength = 0;
                int enemyStrength = 0;
                if (alliesWon == true) allyStrength -= 2;
                if (alliesWon == false) enemyStrength -= 2;
                foreach (Unit unit in allyUnits) allyStrength += unit.GetStrength(); // zvyšuje silu spojencov na základe počtu spojencov v zóne
                foreach (Unit unit in enemyUnits) enemyStrength += unit.GetStrength();
                if (thiszone.IsWallCheck() == true) allyStrength += 3; // ak bojujeme na múre, pridáme viac sily
                if (thiszone.IsWallCheck() && Zone.isWallUpgraded) allyStrength++;  // pridáva viac bojovej sily, ak bola zóna vylepšená lepšími múrmi
                if (thiszone.IsWallCheck() && Zone.WallLevel2) allyStrength++;

                int randomElementally = Random.Range(1, 7);
                int randomElementenemy = Random.Range(1, 7);

                allyStrength += randomElementally;
                enemyStrength += randomElementenemy;

                // Vykonaj logiku eliminácie (napr. zničenie jednotky)
                if (allyStrength > enemyStrength)
                {
                    // Spojenec vyhráva, eliminuj nepriateľskú jednotku
                    Unit enemyUnit = enemyUnits[0];
                    enemyUnits.Remove(enemyUnit);
                    EliminateUnit(enemyUnit);
                    EnemyAI.Instance.HandleUnitDestroyed(enemyUnit);
                    alliesWon = true;
                }
                else if (enemyStrength > allyStrength)
                {
                    // Nepriateľ vyhráva, eliminuj spojeneckú jednotku
                    Unit allyUnit = allyUnits[0];
                    allyUnits.Remove(allyUnit);
                    EliminateUnit(allyUnit);
                    alliesWon = false;
                }
                else
                {
                    // Sily sú rovnaké, obe jednotky sú eliminované
                    Unit allyUnit = allyUnits[0];
                    Unit enemyUnit = enemyUnits[0];
                    allyUnits.Remove(allyUnit);
                    enemyUnits.Remove(enemyUnit);
                    EliminateUnit(enemyUnit);
                    EliminateUnit(allyUnit);
                    EnemyAI.Instance.HandleUnitDestroyed(enemyUnit);
                    alliesWon = null;
                }
                thiszone.ShowBattleProgressBar();
                if (allyUnits.Count == 0)
                {
                    thiszone.ChangeControlToEnemy(); // zmena kontroly zóny, ak všetci nepriatelia sú vymazaní
                    thiszone.HideBattleProgressBar(); // ak jedna strana vyhrá, bojová lišta už nie je potrebná
                    thiszone.ChangeCombatStatus(false);
                    foreach (Unit unit in enemyUnits)
                    {
                        unit.SetShootingAnimation(false);
                    }
                }
                else if (enemyUnits.Count == 0)
                {
                    thiszone.ChangeControlToAlly();
                    thiszone.HideBattleProgressBar();
                    thiszone.ChangeCombatStatus(false);
                    foreach (Unit unit in allyUnits)
                    {
                        unit.SetShootingAnimation(false);
                    }
                }
            }
        }
    }

    private void EliminateUnit(Unit unit)
    {
        // Ďalšia logika na elimináciu jednotky
        if (unit != null) unit.IsDead();
        if (unit.IsEnemy()) unit.SetEnemyPastZoneBack();
        if (!unit.IsEnemy()) unit.SetPastZoneBack();
    }
}
