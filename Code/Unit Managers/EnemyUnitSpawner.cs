using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit SpawnUnitTemplateEnemy;
    [SerializeField] private Unit SpawnStrongerUnitTemplateEnemy;
    [SerializeField] private Zone[] EnemySpawnZones;
    private int strongEnemySpawnerCount;
    public static EnemyUnitSpawner Instance;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one EnemyUnitSpawner! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        strongEnemySpawnerCount = 2;
    }
    public void SpawnEnemyAtTurn()
    {
        List<Zone> zones = new List<Zone>();
        zones.Add(EnemySpawnZones[UnityEngine.Random.Range(0, 4)]);
        zones.Add(EnemySpawnZones[UnityEngine.Random.Range(0, 4)]);
        if (strongEnemySpawnerCount <= 0) zones.Add(EnemySpawnZones[UnityEngine.Random.Range(0, 4)]);
        if (DifficultySetter.GetDifficulty() == "Hard" || DifficultySetter.GetDifficulty() == "Nightmare") zones.Add(EnemySpawnZones[UnityEngine.Random.Range(0, 4)]);
        foreach (Zone zone in zones)
        {
            for (int i = 0; i < zone.GetEnemyMoveLocationStatuses().Length; i++)
            {
                if (!zone.GetEnemyMoveLocationStatuses()[i])
                {
                    if (strongEnemySpawnerCount <= 0)
                    {
                        Vector3 strongSpawnPosition = zone.GetEnemyMoveLocations()[i];
                        zone.SetEnemyPositionStatus(i, true);
                        Unit strongUnit = Instantiate(SpawnStrongerUnitTemplateEnemy, strongSpawnPosition, Quaternion.identity);
                        strongUnit.SetStandingZone(zone, i);
                        strongEnemySpawnerCount = 2;
                    }
                    Vector3 spawnPosition = zone.GetEnemyMoveLocations()[i];
                    zone.SetEnemyPositionStatus(i, true);
                    Unit unit = Instantiate(SpawnUnitTemplateEnemy, spawnPosition, Quaternion.identity);
                    unit.SetStandingZone(zone, i);
                    break;

                }
            }
        }
        strongEnemySpawnerCount--;
    }
}
