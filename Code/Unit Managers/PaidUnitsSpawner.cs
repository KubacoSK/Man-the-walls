using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaidUnitsSpawner : Unit
{
    // for buttons that spawn units
    public void SpawnTankUnit()
    {
        AllyUnitSpawner.Instance.SpawnTank();
    }

    public void SpawnHorseUnit()
    {
        AllyUnitSpawner.Instance.SpawnHorse();
    }

    public void SpawnBattleRobotUnit()
    {
        AllyUnitSpawner.Instance.SpawnBattleRobot();
    }
}
