using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    private float totalPopulation;

    // getters and setters to resources and their incomes
    private int coalCount;
    public int CoalCount { get { return coalCount; } set { coalCount = value; } }
    private int redCryCount;
    public int RedCryCount {  get { return redCryCount; } set { redCryCount = value; } }
    private int blueCryCount;
    public int BlueCryCount { get { return blueCryCount; } set { blueCryCount = value; } }
    private int steelCount;
    public int SteelCount { get { return steelCount; } set { steelCount = value; } }

    private int coalIncome;
    public int CoalIncome {  get { return coalIncome; } set {coalIncome = value; } }
    private int redCIncome;
    public int RedCIncome {  get { return redCIncome; } set { redCIncome = value; } }
    private int bluCIncome;
    public int BluCIncome { get { return bluCIncome; } set { bluCIncome = value; } }
    private int steelIncome;
    public int SteelIncome { get { return steelIncome; } set { steelIncome = value; } }

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
        Zone.ZoneControlChanged += Zone_ZoneControlChanged;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        // sets up system so it shows even on first turn
        foreach (Zone zone in ZoneManager.Instance.ReturnAlliedZones())
        {
            totalPopulation += zone.GetNumberOfCitizens();
        }
        foreach (Zone zone in ZoneManager.Instance.ReturnAlliedZones())
        {
            coalIncome += zone.NumberOfCoal;
            redCIncome += zone.NumberOFRedCrystal;
            bluCIncome += zone.NumberOfBlueCrystal;
            steelIncome += zone.NumberOfSteel;
        }
    }

    private void UpdateResourceIncome(Zone zone)
    {
        // adjusts the income based on zone that was taken
        if (zone.WhoIsUnderControl() == Zone.ControlType.allied)
        {
            coalIncome += zone.NumberOfCoal;
            redCIncome += zone.NumberOFRedCrystal;
            bluCIncome += zone.NumberOfBlueCrystal;
            steelIncome += zone.NumberOfSteel;
        }
        else if (zone.WhoIsUnderControl() == Zone.ControlType.enemy)
        {
            coalIncome -= zone.NumberOfCoal;
            redCIncome -= zone.NumberOFRedCrystal;
            bluCIncome -= zone.NumberOfBlueCrystal;
            steelIncome -= zone.NumberOfSteel;
        }
    }
    private void UpdateResources()
    {
        // Increases number of resources based of income
        coalCount += coalIncome;
        redCryCount += redCIncome;
        blueCryCount += bluCIncome;
        steelCount += steelIncome;
    }

    public void Zone_ZoneControlChanged(object sender, EventArgs e)
    {
        // checks if zone is changed to allied or enemy and removes it from the list
        Zone zone = sender as Zone;
        UpdateResourceIncome(zone);
        if (zone.WhoIsUnderControl() == Zone.ControlType.allied)
            totalPopulation += zone.GetNumberOfCitizens();
        else totalPopulation -= zone.GetNumberOfCitizens();
        // we update population each time the zone is changed
    }

    public void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            // calcualtes total population based on controlled zones
            totalPopulation = 0;
            foreach (Zone zone in ZoneManager.Instance.ReturnAlliedZones())
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

    public bool DoesItHaveEnoughResources(int Steel, int Bcrys, int Rcrys, int coal)
    {
        if (SteelCount >= Steel &&         // we check if we have higher or same amount of resources required
            BlueCryCount >= Bcrys &&
            RedCryCount >= Rcrys &&
            CoalCount >= coal
            )
        {
            return true;
        }
        return false;
    }
}