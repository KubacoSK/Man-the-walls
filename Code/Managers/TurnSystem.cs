using System;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{

    public static TurnSystem Instance { get; private set; }

    public event EventHandler OnTurnChanged;
    private int turnNumber = 1;
    private bool isPlayerTurn = true;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existuje viac ako jeden UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void NextTurn()
    {
        // Prepína medzi ťahmi a zvyšuje číslo ťahu

        if (isPlayerTurn)
        {
            AllyUnitSpawner.Instance.ResetPaidSpawnedUnits();
            AllyUnitSpawner.Instance.SpawnAllyAtTurn();
            turnNumber++;
            if (turnNumber == 15)
            {
                GameManager.Instance.Win();
            }
        }
        isPlayerTurn = !isPlayerTurn;
        // Vyvolá udalosť OnTurnChanged
        OnTurnChanged?.Invoke(this, EventArgs.Empty);
        if (!isPlayerTurn) EnemyUnitSpawner.Instance.SpawnEnemyAtTurn();
        foreach (Zone zone in ZoneManager.GetAllZones())
        {
            // Ak má zóna viac ako jednu jednotku, spustí bojový proces
            zone.InitiateEliminationProcess();
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
