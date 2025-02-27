using System;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    private float totalPopulation;

    // gettery a settery pre suroviny a ich príjmy
    private int coalCount;
    public int CoalCount { get { return coalCount; } set { coalCount = value; } }
    private int redCryCount;
    public int RedCryCount { get { return redCryCount; } set { redCryCount = value; } }
    private int blueCryCount;
    public int BlueCryCount { get { return blueCryCount; } set { blueCryCount = value; } }
    private int steelCount;
    public int SteelCount { get { return steelCount; } set { steelCount = value; } }

    private int coalIncome;
    public int CoalIncome { get { return coalIncome; } set { coalIncome = value; } }
    private int redCIncome;
    public int RedCIncome { get { return redCIncome; } set { redCIncome = value; } }
    private int bluCIncome;
    public int BluCIncome { get { return bluCIncome; } set { bluCIncome = value; } }
    private int steelIncome;
    public int SteelIncome { get { return steelIncome; } set { steelIncome = value; } }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existuje viac ako jeden ResourceManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        Zone.ZoneControlChanged += Zone_ZoneControlChanged;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        // nastaví systém tak, aby sa zobrazoval už v prvom kole
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
        // upraví príjem na základe toho, ktorá zóna bola obsadená
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
        // zvýši množstvo surovín na základe príjmu
        coalCount += coalIncome;
        redCryCount += redCIncome;
        blueCryCount += bluCIncome;
        steelCount += steelIncome;
    }

    public void Zone_ZoneControlChanged(object sender, EventArgs e)
    {
        // kontroluje, či sa zóna zmenila na spojeneckú alebo nepriateľskú, a upraví zoznam
        Zone zone = sender as Zone;
        UpdateResourceIncome(zone);
        if (zone.WhoIsUnderControl() == Zone.ControlType.allied)
            totalPopulation += zone.GetNumberOfCitizens();
        else totalPopulation -= zone.GetNumberOfCitizens();
        // aktualizujeme populáciu pri každej zmene zóny
    }

    public void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            // vypočíta celkovú populáciu na základe ovládaných zón
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
        if (SteelCount >= Steel &&         // kontrolujeme, či máme rovnaké alebo vyššie množstvo požadovaných surovín
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
