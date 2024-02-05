using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{

    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;
    Zone[] allZones;
    private int turnNumber = 1;
    private bool isPlayerTurn = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        allZones = FindObjectsOfType<Zone>();
    }
    public void NextTurn()
    {
        // moves between turns and increases number
        if (isPlayerTurn)
        {
            AllyUnitSpawner.Instance.ResetPaidSpawnedUnits();
            AllyUnitSpawner.Instance.SpawnAllyAtTurn();
            turnNumber++;
            if (turnNumber == 15)
            {
                GameManager.instance.Win();
            }
        }
        isPlayerTurn = !isPlayerTurn;
        // invokes onTurnChanged event
        OnTurnChanged.Invoke(this, EventArgs.Empty);
        if (isPlayerTurn != false) EnemyUnitSpawner.Instance.SpawnEnemyAtTurn();
        foreach (Zone zone in allZones)
        {
            // if zone has more than 1 unit inside it it will do a combat method there
            if (zone.GetUnitsInZone().Count >= 2)
            {
                zone.InitiateEliminationProcess(zone);
            }
        }
        
    }    
    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }
}
