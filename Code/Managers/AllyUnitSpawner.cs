using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class AllyUnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit SpawnUnitTemplate;
    public static AllyUnitSpawner Instance;
    private Vector2 SpawnPos = new Vector2 (14.1f, 17);
    private int PosReset = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SpawnAllyAtTurn()
    {
        SpawnPos += new Vector2(0.4f, 0f);
        Instantiate(SpawnUnitTemplate, SpawnPos, Quaternion.identity);
        PosReset++;
        if (PosReset == 5)
        {
            PosReset = 0;
            SpawnPos += new Vector2(-2f, 0.8f);
        }    
    }
}
