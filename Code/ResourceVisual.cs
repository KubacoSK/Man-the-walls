using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CoalCountText;
    [SerializeField] private TextMeshProUGUI RedCrysCountText;
    [SerializeField] private TextMeshProUGUI BlueCrysCountText;

    [SerializeField] private TextMeshProUGUI CoalIncomeText;
    [SerializeField] private TextMeshProUGUI RedCIncomeText;
    [SerializeField] private TextMeshProUGUI BluCIncomeText;

    private int CoalCount;
    private int RedCryCount;
    private int BlueCryCount;

    private int CoalIncome;
    private int RedCIncome;
    private int BluIncome;
    void Start()
    {
        Zone.ZoneControlChanged += Zone_ZoneControlChanged;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        foreach(Zone zone in ZoneManager.ReturnAlliedZones())
        {
            CoalIncome += zone.NumberOfCoal;
            RedCIncome += zone.NumberOFRedCrystal;
            BluIncome += zone.NumberOfBlueCrystal;
        }
    }

    void Update()
    {
        
    }

    private void Zone_ZoneControlChanged(object sender, EventArgs e)
    {
        Zone zone = sender as Zone;
        UpdateResources(zone);
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        
    }

    private void UpdateResources(Zone zone)
    {
        if (zone.IsUnderAllycont())
        {
            CoalIncome += zone.NumberOfCoal;
            RedCIncome += zone.NumberOFRedCrystal;
            BluIncome += zone.NumberOfBlueCrystal;
        }
        else
        {
            CoalIncome -= zone.NumberOfCoal;
            RedCIncome -= zone.NumberOFRedCrystal;
            BluIncome -= zone.NumberOfBlueCrystal;
        }
        CoalIncomeText.text = "+" + CoalIncome;
        RedCIncomeText.text = "+" + RedCIncome;
        BluCIncomeText.text = "+" + BluIncome;
    }

    private void AddResources()
    {

    }
}
