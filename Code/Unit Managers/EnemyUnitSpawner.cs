using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit SpawnUnitTemplateEnemy;
    [SerializeField] private Zone[] EnemySpawnZones;
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
    }
    public void SpawnEnemyAtTurn()
    {
        List<Zone> zones = new List<Zone>();
        zones.Add(EnemySpawnZones[UnityEngine.Random.Range(0, 4)]);
        zones.Add(EnemySpawnZones[UnityEngine.Random.Range(0, 4)]);
        if (DifficultySetter.GetDifficulty() == "Hard" || DifficultySetter.GetDifficulty() == "Nightmare") zones.Add(EnemySpawnZones[UnityEngine.Random.Range(0, 4)]);
        foreach (Zone zone in zones)
        {
            for (int i = 0; i < zone.GetEnemyMoveLocationStatuses().Length; i++)
            {
                if (!zone.GetEnemyMoveLocationStatuses()[i])
                {
                    Vector3 spawnPosition = zone.GetEnemyMoveLocations()[i];
                    zone.SetEnemyPositionStatus(i, true);
                    Unit unit = Instantiate(SpawnUnitTemplateEnemy, spawnPosition, Quaternion.identity);
                    unit.SetStandingZone(zone, i);
                    break;

                }
            }
        }
    }
}
