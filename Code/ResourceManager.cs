using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    private List<Zone> AlliedControlledZones;

    private float totalPopulation;

    private int coalCount;
    public int CoalCount { get { return coalCount; } }
    private int redCryCount;
    public int RedCryCount {  get { return redCryCount; } }
    private int blueCryCount;
    public int BlueCryCount { get { return blueCryCount; } }
    private int steelCount;
    public int SteelCount { get { return steelCount; } }

    private int coalIncome;
    public int CoalIncome {  get { return coalIncome; } }
    private int redCIncome;
    public int RedCIncome {  get { return redCIncome; } }
    private int bluIncome;
    public int BluIncome { get { return bluIncome; } }
    private int steelIncome;
    public int SteelIncome { get { return steelIncome; } }

    private void Awake()
    {

        if (Instance != null)
        {
            Debug.LogError("There's more than one ResourceManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        AlliedControlledZones = ZoneManager.ReturnAlliedZones();
        Zone.ZoneControlChanged += Zone_ZoneControlChanged;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        // sets up system so it shows even on first turn
        foreach (Zone zone in AlliedControlledZones)
        {
            totalPopulation += zone.GetNumberOfCitizens();
        }
        foreach (Zone zone in ZoneManager.ReturnAlliedZones())
        {
            coalIncome += zone.NumberOfCoal;
            redCIncome += zone.NumberOFRedCrystal;
            bluIncome += zone.NumberOfBlueCrystal;
            steelIncome += zone.NumberOfSteel;
        }
    }

    private void UpdateResourceIncome(Zone zone)
    {
        // adjusts the income based on zone that was taken
        if (zone.IsUnderAllycont())
        {
            coalIncome += zone.NumberOfCoal;
            redCIncome += zone.NumberOFRedCrystal;
            bluIncome += zone.NumberOfBlueCrystal;
            steelIncome += zone.NumberOfSteel;
        }
        else
        {
            coalIncome -= zone.NumberOfCoal;
            redCIncome -= zone.NumberOFRedCrystal;
            bluIncome -= zone.NumberOfBlueCrystal;
            steelIncome -= zone.NumberOfSteel;
        }
    }
    private void UpdateResources()
    {
        // Increases number of resources based of income
        coalCount += coalIncome;
        redCryCount += redCIncome;
        blueCryCount += bluIncome;
        steelCount += steelIncome;
    }

    public void Zone_ZoneControlChanged(object sender, EventArgs e)
    {
        // checks if zone is changed to allied or enemy and removes it from the list
        Zone zone = sender as Zone;
        if (zone.IsUnderAllycont() && !AlliedControlledZones.Contains(zone)) AlliedControlledZones.Add(zone);
        if (!zone.IsUnderAllycont() && AlliedControlledZones.Contains(zone)) AlliedControlledZones.Remove(zone);
        UpdateResourceIncome(zone);
    }

    public void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            // calcualtes total population based on controlled zones
            totalPopulation = 0;
            foreach (Zone zone in AlliedControlledZones)
            {
                zone.PopulationGrowth();
                totalPopulation += zone.GetNumberOfCitizens();  
            }
            UpdateResources();

        }
    }

    public float GetNumberOfTotalPopulation()
    {
        return totalPopulation;
    }
}