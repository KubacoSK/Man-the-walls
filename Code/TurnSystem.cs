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
        if(isPlayerTurn)turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        // invokes onTurnChanged event
        OnTurnChanged.Invoke(this, EventArgs.Empty);
        
        foreach (Zone zone in allZones)
        {
            zone.InitiateEliminationProcess(zone);
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
