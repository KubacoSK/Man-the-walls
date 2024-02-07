using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaidUnitsSpawner : Unit
{

    public void SpawnTankUnit()
    {
        AllyUnitSpawner.Instance.SpawnTank();
    }

    public void SpawnHorse()
    {
        AllyUnitSpawner.Instance.SpawnHorse();
    }

}
