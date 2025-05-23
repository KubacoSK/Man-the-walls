﻿using System;
using UnityEngine;

public class LossesManager : MonoBehaviour
{
    public static LossesManager Instance;

    public int totalAlliedInfantryLosses { get; private set; }
    public int totalAlliedTankLosses { get; private set; }
    public int totalAlliedHorsemanLosses { get; private set; }
    public int totalAlliedBattleRobotLosses { get; private set; }
    public int totalEnemyInfantryLosses { get; private set; }

    public int recentAlliedInfantryLosses { get; private set; }
    public int recentAlliedTankLosses { get; private set; }
    public int recentAlliedHorsemanLosses { get; private set; }
    public int recentAlliedBattleRobotLosses { get; private set; }
    public int recentEnemyInfantryLosses { get; private set; }
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one LossesManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        TurnSystem.Instance.OnTurnChanged += LossesManager_OnTurnChanged;
        Unit.OnAnyUnitDead += LossesManager_OnAnyUnitDeath;    // odober eventov
    }

    private void LossesManager_OnTurnChanged(object sender, EventArgs e)
    {
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            // vynuluje nedávne stráty vždy keď pustíme ďaľšie kolo
            recentAlliedInfantryLosses = 0;
            recentAlliedTankLosses = 0;
            recentAlliedHorsemanLosses = 0;
            recentAlliedBattleRobotLosses = 0;
            recentEnemyInfantryLosses = 0;
        }
    }
    private void LossesManager_OnAnyUnitDeath(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        switch (unit.TypeOfUnit)   // zistíme aký typ jednotky umrel, podľa toho ho priradíme do listu strát
        {
            case Unit.UnitType.Infantry:
                if (!unit.IsEnemy())
                {
                    recentAlliedInfantryLosses++;
                    totalAlliedInfantryLosses++;
                }
                else if (unit.IsEnemy())
                {
                    recentEnemyInfantryLosses++;
                    totalEnemyInfantryLosses++;
                }
                break;
            case Unit.UnitType.Tank:
                recentAlliedTankLosses++;
                totalAlliedTankLosses++;
                break;
            case Unit.UnitType.Horseman:
                recentAlliedHorsemanLosses++;
                totalAlliedHorsemanLosses++;
                break;
            case Unit.UnitType.BattleRobot:
                recentAlliedBattleRobotLosses++;
                totalAlliedBattleRobotLosses++;
                break;
        }
    }
}
