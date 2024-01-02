using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;

public class AllyUnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit SpawnUnitTemplate;
    public static AllyUnitSpawner Instance;

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
        Instantiate(SpawnUnitTemplate, new Vector2(16,16), Quaternion.identity);
    }
}
