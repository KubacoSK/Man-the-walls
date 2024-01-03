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
        Instantiate(SpawnUnitTemplateEnemy, EnemySpawnZones[UnityEngine.Random.Range(0, 4)].transform.position, Quaternion.identity);
        Instantiate(SpawnUnitTemplateEnemy, EnemySpawnZones[UnityEngine.Random.Range(0, 4)].transform.position, Quaternion.identity);
    }
}
