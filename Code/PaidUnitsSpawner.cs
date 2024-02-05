using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PaidUnitsSpawner : MonoBehaviour
{
    private int SpawnUnitLimit = 1;

    public void SpawnTankUnit()
    {
        AllyUnitSpawner.Instance.SpawnTank();
    }

}
